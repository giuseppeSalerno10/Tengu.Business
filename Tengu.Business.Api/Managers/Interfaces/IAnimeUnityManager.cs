using Downla;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;

namespace Tengu.Business.API.Managers.Interfaces
{
    public interface IAnimeUnityManager
    {
        DownloadInfosModel DownloadAsync(string downloadPath, string episodeId, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count = 30, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, int count = 30, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count = 30, CancellationToken cancellationToken = default);
        Task<Calendar> GetCalendar(CancellationToken cancellationToken = default);
    }
}
