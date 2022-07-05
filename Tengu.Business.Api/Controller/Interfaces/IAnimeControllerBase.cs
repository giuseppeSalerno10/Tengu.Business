using Downla;
using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;

namespace Tengu.Business.API.Controller.Interfaces
{
    public interface IAnimeControllerBase
    {
        void UpdateDownlaSettings(string? downloadPath, int maxConnections, long maxPacketSize);

        OperationResult<DownloadMonitor> DownloadAsync(string episodeUrl, out Task downloadTask, CancellationToken cancellationToken);
        Task<OperationResult<Calendar>> GetCalendar(CancellationToken cancellationToken);
        Task<OperationResult<EpisodeModel[]>> GetEpisodesAsync(string animeId, int offset, int limit, CancellationToken cancellationToken);
        Task<OperationResult<EpisodeModel[]>> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken);
        Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(SearchFilter filter, int count, CancellationToken cancellationToken);
        Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken);
        Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(string title, SearchFilter filter, int count, CancellationToken cancellationToken);
    }
}
