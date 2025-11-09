using RefTrackSearcher.Core.Interfaces.Services;
using RefTrackSearcher.Core.Models;
using System.Text.Json;

namespace RefTrackSearcher.Infrastructure.Services
{


    public class JamendoService: IJamendoService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.jamendo.com/v3.0/tracks/";

        public JamendoService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ApiResponse> GetTracksAsync(string сlientId, TrackQueryParams parameters = null)
        {
            try
            {
                parameters ??= new TrackQueryParams();

                var queryString = BuildQueryString(parameters, сlientId);
                var url = $"{BaseUrl}?{queryString}";

                Console.WriteLine($"Request URL: {url}");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return apiResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private string BuildQueryString(TrackQueryParams parameters, string сlientId)
        {
            var queryParams = new Dictionary<string, string>
            {
                ["client_id"] = сlientId,
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
