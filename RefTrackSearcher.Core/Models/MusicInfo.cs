using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RefTrackSearcher.Core.Models
{
    public class MusicInfo
    {
        [JsonPropertyName("vocalinstrumental")]
        public string VocalInstrumental { get; set; }

        [JsonPropertyName("lang")]
        public string Language { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("acousticelectric")]
        public string AcousticElectric { get; set; }

        [JsonPropertyName("speed")]
        public string Speed { get; set; }

        [JsonPropertyName("tags")]
        public Tags Tags { get; set; }
    }
}
