using FluentAssertions;
using Application.Locations.Queries.GetCharactersByLocation;

namespace Application.IntegrationTests.Locations.Queries;

using static Testing;

public class GetCharactersByLocationTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnCharactersWithLocation()
    {
        // Arrange
        var locations = Entities.GetLocations();
        await AddAsync(locations);

        var characters = Entities.GetCharacters(locations.First().Id);
        await AddAsync(characters);

        var query = new GetCharactersByLocationQuery
        {
            Location = "Earth"
        };

        // Act
        var result = await SendAsync(query);

        // Assert
        result.Characters.TotalCount.Should().Be(characters.Count);
    }

    [Test]
    public async Task ShouldReturnEmptyWhenNoCharactersInDatabase()
    {
        var query = new GetCharactersByLocationQuery
        {
            Location = "Earth"
        };

        var result = await SendAsync(query);

        result.Characters.TotalCount.Should().Be(0);
        result.Characters.Items.Should().BeEmpty();
    }

    [Test]
    public async Task ShouldReturnFromCacheOnSecondRequest()
    {
        // Arrange
        var locations = Entities.GetLocations();
        await AddAsync(locations);

        var characters = Entities.GetCharacters(locations.First().Id);
        await AddAsync(characters);

        var query = new GetCharactersByLocationQuery
        {
            Location = "Earth"
        };

        // Act
        var firstResult = await SendAsync(query); // Should populate cache
        var secondResult = await SendAsync(query); // Should return from cache

        // Assert
        firstResult.IsFetchedFromDatabase.Should().BeTrue();
        secondResult.IsFetchedFromDatabase.Should().BeFalse();
        secondResult.Characters.TotalCount.Should().Be(characters.Count);
    }
}