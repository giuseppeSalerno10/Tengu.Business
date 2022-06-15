using Downla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.Commons;

namespace Tengu.Business.API.Controller
{
    public class AnimeSaturnController : IAnimeSaturnController
    {
        private readonly IAnimeSaturnManager _manager;

        public AnimeSaturnController(IAnimeSaturnManager manager)
        {
            _manager = manager;
        }

        public Task<AnimeModel[]> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken)
        {
            try
            {
                return _manager.SearchAnimeAsync(title, count, cancellationToken);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count, CancellationToken cancellationToken)
        {
            return _manager.SearchAnimeAsync(filter, count, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count, CancellationToken cancellationToken)
        {
            return _manager.SearchAnimeAsync(title, filter, count, cancellationToken);
        }

        public Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset, int limit, CancellationToken cancellationToken)
        {
            return _manager.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
        }

        public Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken)
        {
            return _manager.GetLatestEpisodesAsync(offset, limit, cancellationToken);
        }

        public Task<Calendar> GetCalendar(CancellationToken cancellationToken)
        {
            return _manager.GetCalendar(cancellationToken);
        }

        public DownloadInfosModel DownloadAsync(string downloadPath, string episodeUrl, CancellationToken cancellationToken)
        {
            return _manager.DownloadAsync(downloadPath, episodeUrl, cancellationToken);
        }
    }
}
