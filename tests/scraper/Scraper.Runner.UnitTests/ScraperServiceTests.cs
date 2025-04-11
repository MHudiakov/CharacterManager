using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Moq;
using Scraper.Runner.Scraper;
using Domain.Entities;
using Moq.EntityFrameworkCore;
using Scraper.ApiClient;
using Scraper.ApiClient.Models;
using AutoMapper;
using Domain.Enums;
using Scraper.ApiClient.Exceptions;
using System.Net;

namespace Scraper.Runner.UnitTests;

public class ScraperServiceTests
{
    private ILogger<ScraperService> _logger = null!;

    private IMapper _mapper = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<ScraperService>>().Object;
        var mapperMock = new Mock<IMapper>();

        mapperMock.Setup(m => m.Map<Character>(It.IsAny<CharacterDto>()))
            .Returns((CharacterDto dto) => new Character
            {
                Id = dto.Id,
                Name = dto.Name,
                Gender = (Gender)Enum.Parse(typeof(Gender), dto.Gender),
                Species = dto.Species
            });

        mapperMock.Setup(m => m.Map<Location>(It.IsAny<LocationDto>()))
            .Returns((LocationDto dto) => new Location
            {
                Name = dto.Name,
                Type = dto.Type
            });

        _mapper = mapperMock.Object;
    }

    [Test]
    public async Task WhenDataIsCorrect_LoadsAllDataToDb()
    {
        var characters = new List<Character>();
        var locations = new List<Location>();

        var dbContextMock = BuildDbContextMock(characters, locations);

        var apiClientMock = BuildApiClientMock();

        var service = new ScraperService(apiClientMock.Object, dbContextMock.Object, _logger, _mapper);

        await service.RunAsync(CancellationToken.None);

        characters.Count.Should().Be(2);
    }

    [Test]
    public async Task WhenApiFails_ShouldThrowException()
    {
        var dbContextMock = BuildDbContextMock(new List<Character>(), new List<Location>());

        var apiClientMock = new Mock<IRickAndMortyApiClient>();
        apiClientMock.Setup(x => x.GetCharactersAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ApiException("API failed", HttpStatusCode.InternalServerError));

        var service = new ScraperService(apiClientMock.Object, dbContextMock.Object, _logger, _mapper);

        Assert.ThrowsAsync<ApiException>(async () => await service.RunAsync());
    }

    [Test]
    public async Task WhenApiReturnsNoCharacters_ShouldNotSaveAnything()
    {
        var characters = new List<Character>();
        var dbContextMock = BuildDbContextMock(characters, new List<Location>());

        var emptyCharactersResponseDto = new CharactersResponseDto
        {
            Results = new List<CharacterDto>(),
            Info = new InfoDto
            {
                Pages = 1
            }
        };

        var apiClientMock = BuildApiClientMock(emptyCharactersResponseDto);

        var service = new ScraperService(apiClientMock.Object, dbContextMock.Object, _logger, _mapper);

        await service.RunAsync();

        characters.Should().BeEmpty();
    }

    private Mock<IRickAndMortyApiClient> BuildApiClientMock(CharactersResponseDto? charactersResponseDto = null)
    {
        var apiClientMock = new Mock<IRickAndMortyApiClient>();

        apiClientMock.Setup(x => x.GetCharactersAsync(It.IsAny<int>(), CancellationToken.None))
            .ReturnsAsync(charactersResponseDto ?? GetFakeCharactersResponseDto);

        apiClientMock.Setup(x => x.GetLocationAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(GetFakeLocationDto);

        return apiClientMock;
    }

    private static Mock<ApplicationDbContext> BuildDbContextMock(List<Character> characters, List<Location> locations)
    {
        var dbContextMock = new Mock<ApplicationDbContext>();

        dbContextMock.Setup(x => x.Characters).ReturnsDbSet(characters);

        dbContextMock.Setup(x => x.Locations).ReturnsDbSet(locations);

        dbContextMock.Setup(x => x.Characters.AddRangeAsync(It.IsAny<Character[]>(), CancellationToken.None))
            .Callback(new Action<IEnumerable<Character>, CancellationToken>((characterCollection, _) =>
            {
                var characterList = characterCollection.ToList();
                characters.AddRange(characterList);
            }));

        return dbContextMock;
    }

    private CharactersResponseDto GetFakeCharactersResponseDto => new()
    {
        Results =
        [
            new()
            {
                Id = 1,
                Gender = "Male",
                Name = "Ben",
                Species = "Human",
                Location = new CharacterLocationDto
                {
                    Name = "Earth",
                    Url = "url"
                }
            },

            new()
            {
                Id = 2,
                Gender = "Female",
                Name = "Lisa",
                Species = "Human",
                Location = new CharacterLocationDto
                {
                    Name = "Earth",
                    Url = "url"
                }
            }
        ],
        Info = new InfoDto
        {
            Count = 2,
            Pages = 1
        }
    };

    private LocationDto GetFakeLocationDto => new()
    {
        Name = "Earth",
        Type = "Planet",
        Dimension = "Replacement Dimension"
    };
}