using System.Net.Http.Json;
using Domain.Configuration;
using Domain.ExternalModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Interface;

namespace Service.Implementation;

public class OpenLibraryApiClient : IOpenLibraryApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenLibraryApiClient> _logger;

    public OpenLibraryApiClient(HttpClient httpClient, ILogger<OpenLibraryApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    public async Task<OpenLibrarySearchResponse> SearchByCategoryAsync(string category, int limit = 5)
    {
        var url = $"search.json?q=subject:{Uri.EscapeDataString(category)}&limit={limit}";

        _logger.LogInformation("Calling Open Library API: {Url}", url);

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OpenLibrarySearchResponse>();
        return result ?? new OpenLibrarySearchResponse();
    }
}