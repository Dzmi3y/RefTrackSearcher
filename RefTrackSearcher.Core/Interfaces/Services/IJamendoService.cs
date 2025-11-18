using RefTrackSearcher.Core.Models;

namespace RefTrackSearcher.Core.Interfaces.Services
{
    public interface IJamendoService
    {
        Task<ApiResponse?> GetTracksAsync(TrackQueryParams parameters = null);
        
    }
}
