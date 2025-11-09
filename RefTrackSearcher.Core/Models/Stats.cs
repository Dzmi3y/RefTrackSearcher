using System.Text.Json.Serialization;

namespace RefTrackSearcher.Core.Models
{
    public class Stats
    {
        [JsonPropertyName("rate_downloads_total")]
        public long RateDownloadsTotal { get; set; }

        [JsonPropertyName("rate_listened_total")]
        public long RateListenedTotal { get; set; }

        [JsonPropertyName("playlisted")]
        public int Playlisted { get; set; }

        [JsonPropertyName("favorited")]
        public int Favorited { get; set; }

        [JsonPropertyName("likes")]
        public int Likes { get; set; }

        [JsonPropertyName("dislikes")]
        public int Dislikes { get; set; }

        [JsonPropertyName("avgnote")]
        public double AvgNote { get; set; }

        [JsonPropertyName("notes")]
        public int Notes { get; set; }
    }
}
