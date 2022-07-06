using Downla;
using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;

namespace Tengu.Business.API.Managers.Interfaces
{
    public interface IAnimeUnityManager
    {
        DownloadMonitor DownloadAsync(string episodeId, out Task downloadTask, CancellationToken cancellationToken);
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset, int count, CancellationToken cancellationToken);
        Task<AnimeModel[]> SearchAnimeAsync(TenguSearchFilter filter, int count, CancellationToken cancellationToken);
        Task<AnimeModel[]> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken);
        Task<AnimeModel[]> SearchAnimeAsync(string title, TenguSearchFilter filter, int count, CancellationToken cancellationToken);
        Task<Calendar> GetCalendar(CancellationToken cancellationToken);
        void UpdateDownlaSettings(string? downloadPath, int maxConnections, long maxPacketSize);
    }
}
