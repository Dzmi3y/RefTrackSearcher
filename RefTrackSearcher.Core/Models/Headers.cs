using System.Text.Json.Serialization;

namespace RefTrackSearcher.Core.Models
{
    public class Headers
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("warnings")]
        public string Warnings { get; set; }

        [JsonPropertyName("results_count")]
        public int ResultsCount { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }
    }

}
