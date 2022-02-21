using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Core;

namespace Tengu.Business.API
{
    public class AnimeUnityManager : IAnimeUnityManager
    {
        private readonly IAnimeUnityAdapter _adapter;
        private readonly IAnimeUnityUtilities _utilities;

        public AnimeUnityManager(IAnimeUnityAdapter adapter, IAnimeUnityUtilities utilities)
        {
            _adapter = adapter;
            _utilities = utilities;
        }

        public Task DownloadAsync(string downloadPath, string episodeId, CancellationToken cancellationToken = default)
        {
            return _adapter.DownloadAsync(downloadPath, episodeId, cancellationToken);
        }

        public Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            return _adapter.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
        }

        public Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken = default)
        {
            return _adapter.GetLatestEpisodesAsync(offset, limit, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, CancellationToken cancellationToken = default)
        {
            var adapterFilter = new AnimeSaturnSearchFilterInput();

            return _adapter.SearchByFiltersAsync(adapterFilter, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnimeAsync(string title, CancellationToken cancellationToken = default)
        {
            return _adapter.SearchByTitleAsync(title, cancellationToken);
        }

        public async Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, CancellationToken cancellationToken = default)
        {
            var adapterFilter = new AnimeSaturnSearchFilterInput();

            var result = await _adapter.SearchByFiltersAsync(adapterFilter, cancellationToken);

            return result
                .Where(anime => anime.Title.Contains(title))
                .ToArray();
        }
    }
}
