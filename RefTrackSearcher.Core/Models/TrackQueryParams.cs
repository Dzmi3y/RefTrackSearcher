using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefTrackSearcher.Core.Models
{
    public class TrackQueryParams
    {
        public string Tags { get; set; }
        public int Limit { get; set; } = 10;
        public int Offset { get; set; } = 0;
        public string Order { get; set; } = "popularity_total";
        public string Include { get; set; } = "musicinfo+stats";
        public string Name { get; set; }
        public string ArtistName { get; set; }
        public string Id { get; set; }
    }
}
