using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RefTrackSearcher.Core.Data
{
    public class Instruments
    {
        public List<string> Strings { get; set; }
        public List<string> Keyboards { get; set; }
        public List<string> Percussion { get; set; }
        public List<string> Wind { get; set; }
        public List<string> Other { get; set; }
    }
}
