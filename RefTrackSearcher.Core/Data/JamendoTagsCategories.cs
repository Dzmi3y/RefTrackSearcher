using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RefTrackSearcher.Core.Data
{
    public class JamendoTagsCategories
    {
        public List<string> Genres { get; set; }
        public List<string> Moods { get; set; }
        public Instruments Instruments { get; set; }
        public List<string> Vocals { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Technical { get; set; }
    }
}
