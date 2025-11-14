using System.Text.Json.Serialization;

namespace RefTrackSearcher.Core.Data
{
    public class JamendoTagsRoot
    {
        [JsonPropertyName("jamendo_tags_categories")]
        public JamendoTagsCategories Jamendo_Tags_Categories { get; set; }
    }
}
