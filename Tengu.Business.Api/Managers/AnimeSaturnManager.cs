using Downla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Core;

namespace Tengu.Business.API
{
    public class AnimeSaturnManager : IAnimeSaturnManager
    {
        private readonly IAnimeSaturnAdapter _adapter;
        private readonly IAnimeSaturnUtilities _utilities;
        private readonly IDownlaClient _downlaClient;

        public AnimeSaturnManager(IAnimeSaturnAdapter adapter, IAnimeSaturnUtilities utilities, IDownlaClient downlaClient)
        {
            _adapter = adapter;
            _utilities = utilities;
            _downlaClient = downlaClient;
        }

        public DownloadInfosModel DownloadAsync(string downloadPath, string episodeUrl, CancellationToken cancellationToken = default)
        {
            _downlaClient.DownloadPath = downloadPath;
            return _downlaClient.StartDownload(new Uri(episodeUrl), cancellationToken);
        }

        public Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            return _adapter.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
        }

        public Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            return _adapter.GetLatestEpisodesAsync(offset, limit, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count = 30, CancellationToken cancellationToken = default)
        {
            var adapterFilter = new AnimeSaturnSearchFilterInput()
            {
                Genres = _utilities.GetGenreArray(filter.Genres),
            };

            if(filter.Status != Statuses.None)
            {
                adapterFilter.Status = _utilities.GetStatus(filter.Status);
            }
            if (!string.IsNullOrEmpty(filter.Year))
            {
                adapterFilter.Year = filter.Year;
            }

            return _adapter.SearchByFiltersAsync(adapterFilter, count, cancellationToken);

        }

        public Task<AnimeModel[]> SearchAnimeAsync(string title, int count = 30, CancellationToken cancellationToken = default)
        {
            return _adapter.SearchByTitleAsync(title, count, cancellationToken);
        }

        public async Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count = 30, CancellationToken cancellationToken = default)
        {
            var animeList = await SearchAnimeAsync(filter, count, cancellationToken);

            return animeList
                .Where(anime => anime.Title.Contains(title))
                .ToArray();
        }
    }
}
