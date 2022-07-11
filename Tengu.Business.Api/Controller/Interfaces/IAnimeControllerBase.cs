using Downla;
using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;

namespace Tengu.Business.API.Controller.Interfaces
{
    public interface IAnimeControllerBase
    {
        Task<OperationResult<DownloadMonitor>> StartDownloadAsync(string episodeUrl, CancellationToken cancellationToken);
        Task<OperationResult<Calendar>> GetCalendar(CancellationToken cancellationToken);
        Task<OperationResult<EpisodeModel[]>> GetEpisodesAsync(string animeId, int offset, int count, CancellationToken cancellationToken);
        Task<OperationResult<EpisodeModel[]>> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken);
        Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(TenguSearchFilter filter, int count, CancellationToken cancellationToken);
        Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken);
        Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(string title, TenguSearchFilter filter, int count, CancellationToken cancellationToken);
    }
}
