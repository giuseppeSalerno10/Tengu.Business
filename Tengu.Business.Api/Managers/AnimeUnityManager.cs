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

        public Task Download(string downloadPath, string episodeId, CancellationToken cancellationToken = default)
        {
            return _adapter.Download(downloadPath, episodeId, cancellationToken);
        }

        public Task<EpisodeModel[]> GetEpisodes(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            return _adapter.GetEpisodes(animeId, offset, limit, cancellationToken);
        }

        public Task<EpisodeModel[]> GetLatestEpisodes(int offset, int limit, CancellationToken cancellationToken = default)
        {
            return _adapter.GetLatestEpisodes(offset, limit, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnime(SearchFilter filter, CancellationToken cancellationToken = default)
        {
            var adapterFilter = new AnimeSaturnSearchFilterInput();

            return _adapter.SearchByFilters(adapterFilter, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnime(string title, CancellationToken cancellationToken = default)
        {
            return _adapter.SearchByTitle(title, cancellationToken);
        }

        public async Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter, CancellationToken cancellationToken = default)
        {
            var adapterFilter = new AnimeSaturnSearchFilterInput();

            var result = await _adapter.SearchByFilters(adapterFilter, cancellationToken);

            return result
                .Where(anime => anime.Title.Contains(title))
                .ToArray();
        }
    }
}
