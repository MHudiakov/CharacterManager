using System.Text.Json;
using Scraper.ApiClient.Exceptions;
using Scraper.ApiClient.Models;

namespace Scraper.ApiClient;

public class RickAndMortyApiClient : IRickAndMortyApiClient
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _options;

    public RickAndMortyApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<CharactersResponseDto> GetCharactersAsync(int page, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync($"/api/character?status=alive&page={page}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException("Attempt to retrieve characters was not successful", response.StatusCode);
        }

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var charactersResponseDto = await JsonSerializer.DeserializeAsync<CharactersResponseDto>(stream, options: _options, cancellationToken);
        return charactersResponseDto!;
    }

    public async Task<LocationDto> GetLocationAsync(string url, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException("Attempt to retrieve location was not successful", response.StatusCode);
        }

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var locationDto = await JsonSerializer.DeserializeAsync<LocationDto>(stream, options: _options, cancellationToken);

        return locationDto!;
    }
}