using System.Text.Json.Serialization;

namespace RefTrackSearcher.Core.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("headers")]
        public Headers Headers { get; set; }

        [JsonPropertyName("results")]
        public List<Track> Results { get; set; }
    }
}
