using Downla;
using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;

namespace Tengu.Business.API.Managers.Interfaces
{
    public interface IAnimeSaturnManager
    {
        DownloadMonitor DownloadAsync(string episodeUrl, out Task downloadTask, CancellationToken cancellationToken );
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken );
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset, int limit, CancellationToken cancellationToken );
        Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count, CancellationToken cancellationToken );
        Task<AnimeModel[]> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken );
        Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count, CancellationToken cancellationToken );
        Task<Calendar> GetCalendar(CancellationToken cancellationToken );
        void UpdateDownlaSettings(string? downloadPath, int maxConnections, long maxPacketSize);
    }
}
