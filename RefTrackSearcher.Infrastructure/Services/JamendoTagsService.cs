using RefTrackSearcher.Core.Data;
using RefTrackSearcher.Core.Interfaces.Services;
using System.ComponentModel;

namespace RefTrackSearcher.Infrastructure.Services
{
    public class JamendoTagsService : IJamendoTagsService
    {
        private readonly JamendoTagsRoot _tagsData;

        public JamendoTagsService(JamendoTagsRoot tagsData)
        {
            _tagsData = tagsData;
        }

        public List<string> GetGenres() => _tagsData.Jamendo_Tags_Categories.Genres;
       
    }
}
