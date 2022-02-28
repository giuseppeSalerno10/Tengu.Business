using Downla;
using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface IAnimeSaturnManager
    {
        DownloadInfosModel DownloadAsync(string downloadPath, string episodeUrl, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count = 30, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, int count = 30, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count = 30, CancellationToken cancellationToken = default);
        Task<Calendar> GetCalendar(CancellationToken cancellationToken = default);
    }
}
