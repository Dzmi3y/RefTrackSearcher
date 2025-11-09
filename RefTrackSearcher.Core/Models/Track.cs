using System.Text.Json.Serialization;

namespace RefTrackSearcher.Core.Models
{
    public class Track
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("artist_id")]
        public string ArtistId { get; set; }

        [JsonPropertyName("artist_name")]
        public string ArtistName { get; set; }

        [JsonPropertyName("artist_idstr")]
        public string ArtistIdStr { get; set; }

        [JsonPropertyName("album_name")]
        public string AlbumName { get; set; }

        [JsonPropertyName("album_id")]
        public string AlbumId { get; set; }

        [JsonPropertyName("license_ccurl")]
        public string LicenseCcUrl { get; set; }

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("releasedate")]
        public string ReleaseDate { get; set; }

        [JsonPropertyName("album_image")]
        public string AlbumImage { get; set; }

        [JsonPropertyName("audio")]
        public string Audio { get; set; }

        [JsonPropertyName("audiodownload")]
        public string AudioDownload { get; set; }

        [JsonPropertyName("prourl")]
        public string ProUrl { get; set; }

        [JsonPropertyName("shorturl")]
        public string ShortUrl { get; set; }

        [JsonPropertyName("shareurl")]
        public string ShareUrl { get; set; }

        [JsonPropertyName("waveform")]
        public string Waveform { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("musicinfo")]
        public MusicInfo MusicInfo { get; set; }

        [JsonPropertyName("stats")]
        public Stats Stats { get; set; }

        [JsonPropertyName("audiodownload_allowed")]
        public bool AudioDownloadAllowed { get; set; }

        [JsonPropertyName("content_id_free")]
        public bool ContentIdFree { get; set; }
    }
}
