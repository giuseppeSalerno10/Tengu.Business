using Downla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Objects;

namespace Tengu.Business.API.Controller.Interfaces
{
    public interface IAnimeControllerBase
    {
        TenguResult<DownloadInfosModel> DownloadAsync(string downloadPath, string episodeUrl, CancellationToken cancellationToken);
        Task<TenguResult<Calendar>> GetCalendar(CancellationToken cancellationToken);
        Task<TenguResult<EpisodeModel[]>> GetEpisodesAsync(string animeId, int offset, int limit, CancellationToken cancellationToken);
        Task<TenguResult<EpisodeModel[]>> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken);
        Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(SearchFilter filter, int count, CancellationToken cancellationToken);
        Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken);
        Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(string title, SearchFilter filter, int count, CancellationToken cancellationToken);
    }
}
