using System.Text.Json;
using RefTrackSearcher.Core.Interfaces.Services;
using RefTrackSearcher.Core.Models;
using Microsoft.Extensions.Options;
using RefTrackSearcher.Core.Config;

namespace RefTrackSearcher.Infrastructure.Services
{
    public class JamendoService : IJamendoService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.jamendo.com/v3.0/tracks/";
        private readonly IOptions<JamendoConfig> _options;
        private readonly IFileCacheService _cacheService;

        public JamendoService(IOptions<JamendoConfig> options, IFileCacheService cacheService)
        {
            _httpClient = new HttpClient();
            _options = options;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse?> GetTracksAsync(TrackQueryParams parameters = null)
        {
            try
            {
                parameters ??= new TrackQueryParams();
                var cacheKey = _cacheService.GenerateKey(
                    parameters.Limit, parameters.Offset, parameters.Order, 
                    parameters.Include, parameters.Tags, parameters.Name, 
                    parameters.ArtistName, parameters.Id);

                return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
                {
                    var queryString = BuildQueryString(parameters);
                    var url = $"{BaseUrl}?{queryString}";

                    Console.WriteLine($"Request URL: {url}");

                    var response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private string BuildQueryString(TrackQueryParams parameters)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["client_id"] = _options.Value.ClientId,
                ["format"] = "json",
                ["limit"] = parameters.Limit.ToString(),
                ["offset"] = parameters.Offset.ToString(),
                ["order"] = parameters.Order,
                ["include"] = parameters.Include
            };

            if (!string.IsNullOrEmpty(parameters.Tags))
                queryParams["tags"] = parameters.Tags;

            if (!string.IsNullOrEmpty(parameters.Name))
                queryParams["name"] = parameters.Name;

            if (!string.IsNullOrEmpty(parameters.ArtistName))
                queryParams["artist_name"] = parameters.ArtistName;

            if (!string.IsNullOrEmpty(parameters.Id))
                queryParams["id"] = parameters.Id;

            return string.Join("&", queryParams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
        }
    }
}
