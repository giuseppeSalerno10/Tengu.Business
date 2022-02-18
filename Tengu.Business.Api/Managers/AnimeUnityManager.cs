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

        public Task Download(string downloadPath, EpisodeModel episode, CancellationToken cancellationToken = default)
        {
            return _adapter.Download(downloadPath, episode.Url, cancellationToken);
        }

        public Task<EpisodeModel[]> GetLatestEpisode(int count, CancellationToken cancellationToken = default)
        {
            return _adapter.GetLatestEpisode(count, cancellationToken);
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
                .Where(anime => anime.Title.Contains(title) || anime.AlternativeTitle.Contains(title))
                .ToArray();
        }
    }
}
