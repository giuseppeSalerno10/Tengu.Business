using Downla;
using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;

namespace Tengu.Business.API.Managers.Interfaces
{
    public interface IAnimeSaturnManager
    {
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken );
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset, int count, CancellationToken cancellationToken );
        Task<AnimeModel[]> SearchAnimeAsync(TenguSearchFilter filter, int count, CancellationToken cancellationToken );
        Task<AnimeModel[]> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken );
        Task<AnimeModel[]> SearchAnimeAsync(string title, TenguSearchFilter filter, int count, CancellationToken cancellationToken );
        Task<Calendar> GetCalendar(CancellationToken cancellationToken );
        Task<DownloadMonitor> StartDownloadAsync(string episodeUrl, CancellationToken cancellationToken);
    }
}
