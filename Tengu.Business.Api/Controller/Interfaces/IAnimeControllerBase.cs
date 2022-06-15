using Downla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.API.Controller.Interfaces
{
    public interface IAnimeControllerBase
    {
        DownloadInfosModel DownloadAsync(string downloadPath, string episodeUrl, CancellationToken cancellationToken);
        Task<Calendar> GetCalendar(CancellationToken cancellationToken);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset, int limit, CancellationToken cancellationToken);
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken);
        Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count, CancellationToken cancellationToken);
        Task<AnimeModel[]> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken);
        Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count, CancellationToken cancellationToken);
    }
}
