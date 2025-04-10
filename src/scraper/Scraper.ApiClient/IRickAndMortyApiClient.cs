using Scraper.ApiClient.Models;

namespace Scraper.ApiClient;

public interface IRickAndMortyApiClient
{
    Task<CharactersResponseDto> GetCharactersAsync(int page, CancellationToken cancellationToken = default);

    Task<LocationDto> GetLocationAsync(string url, CancellationToken cancellationToken = default);
}