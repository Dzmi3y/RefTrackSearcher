using System.Text.Json.Serialization;

namespace RefTrackSearcher.Core.Models
{
    public class Tags
    {
        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        [JsonPropertyName("instruments")]
        public List<string> Instruments { get; set; }

        [JsonPropertyName("vartags")]
        public List<string> VarTags { get; set; }
    }
}
