using Application.Contracts;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Scraper.ApiClient;
using Scraper.ApiClient.Models;

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

        var page = 1;

        while (true)
        {
            // Characters also might be loaded via Task.WhenAll() as locations, but here it won't provide significant performance improvement
            var characterResponse = await _rickAndMortyApiClient.GetCharactersAsync(page, cancellationToken);

            var characterEnrichmentTasks = characterResponse.Results.Select(character 
                => EnrichCharacterWithLocation(character, cancellationToken)).ToList();
            
            var characters = await Task.WhenAll(characterEnrichmentTasks);

            await _context.Characters.AddRangeAsync(characters, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            _log.LogInformation($"Batch {page} processed with {characters.Length} entries.");
            
            if (page == characterResponse.Info.Pages)
            {
                _log.LogInformation("All entries have been processed.");
                break;
            }

            page++;
        }
    }

    private async Task<Character> EnrichCharacterWithLocation(CharacterDto characterDto, CancellationToken cancellationToken)
    {
        Location? location = null;

        // In case on unknown location the location url is empty
        if (!string.IsNullOrWhiteSpace(characterDto.Location.Url)) 
        {
            var locationDto = await _rickAndMortyApiClient.GetLocationAsync(characterDto.Location.Url, cancellationToken);
            location = _mapper.Map<Location>(locationDto);
        }

        var character = _mapper.Map<Character>(characterDto);

        character.Location = location;

        return character;
    }
}