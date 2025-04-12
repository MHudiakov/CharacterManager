using Application.Contracts;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Scraper.ApiClient;
using System.Collections.Concurrent;
using CharacterDto = Scraper.ApiClient.Models.CharacterDto;

namespace Scraper.Runner.Scraper;

public class ScraperService : IScraperService
{
    private readonly IRickAndMortyApiClient _rickAndMortyApiClient;

    private readonly IApplicationDbContext _context;

    private readonly IMapper _mapper;

    private readonly ILogger<ScraperService> _log;

    public ScraperService(IRickAndMortyApiClient rickAndMortyApiClient, IApplicationDbContext context, ILogger<ScraperService> log, IMapper mapper)
    {
        _rickAndMortyApiClient = rickAndMortyApiClient;
        _context = context;
        _log = log;
        _mapper = mapper;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _log.LogInformation("Clean tables before saving new data...");

        _context.Characters.RemoveRange(_context.Characters);
        _context.Locations.RemoveRange(_context.Locations);
        await _context.SaveChangesAsync(cancellationToken);

        _log.LogInformation("Starting scraping...");

        var savedLocationIds = new List<int>();
        var locationsDictionary = new ConcurrentDictionary<string, Location>(); // Track locations by Url
        var page = 1;

        while (true)
        {
            // Characters also might be loaded via Task.WhenAll() as locations, but here it won't provide significant performance improvement
            var characterResponse = await _rickAndMortyApiClient.GetCharactersAsync(page, cancellationToken);

            var characterEnrichmentTasks = characterResponse.Results.Select(character
                => EnrichCharacterWithLocation(character, locationsDictionary, cancellationToken)).ToList();

            var characters = await Task.WhenAll(characterEnrichmentTasks);

            var newLocations = locationsDictionary.Values.Where(l => !savedLocationIds.Contains(l.Id)).ToList();

            await _context.Locations.AddRangeAsync(newLocations, cancellationToken);
            await _context.Characters.AddRangeAsync(characters, cancellationToken);
            
            await _context.SaveChangesAsync(cancellationToken);

            savedLocationIds.AddRange(newLocations.Select(l => l.Id).ToList());

            _log.LogInformation($"Batch {page} processed with {characters.Length} entries.");

            if (page == characterResponse.Info.Pages)
            {
                _log.LogInformation("All entries have been processed.");
                break;
            }

            page++;
        }
    }

    private async Task<Character> EnrichCharacterWithLocation(
        CharacterDto characterDto, 
        ConcurrentDictionary<string, Location> locationsDictionary, 
        CancellationToken cancellationToken)
    {
        Location? location = null;

        // In case on unknown location the location url is empty
        if (!string.IsNullOrWhiteSpace(characterDto.Location.Url))
        {
            if (!locationsDictionary.TryGetValue(characterDto.Location.Url, out location))
            {
                var locationDto = await _rickAndMortyApiClient.GetLocationAsync(characterDto.Location.Url, cancellationToken);
                location = _mapper.Map<Location>(locationDto);
                locationsDictionary[characterDto.Location.Url] = location;
            }
        }

        var character = _mapper.Map<Character>(characterDto);

        if (location is not null)
        {
            character.LocationId = location.Id;
        }

        return character;
    }
}