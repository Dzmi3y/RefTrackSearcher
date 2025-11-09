using RefTrackSearcher.Core.Models;

namespace RefTrackSearcher.Core.Interfaces.Services
{
    public interface IJamendoService
    {
        Task<ApiResponse> GetTracksAsync(string сlientId, TrackQueryParams parameters = null);
    }
}
