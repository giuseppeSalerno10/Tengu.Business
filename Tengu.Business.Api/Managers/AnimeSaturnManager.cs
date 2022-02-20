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

        public AnimeSaturnManager(IAnimeSaturnAdapter adapter, IAnimeSaturnUtilities utilities)
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

        public async Task<AnimeModel[]> SearchAnime(SearchFilter filter, CancellationToken cancellationToken = default)
        {
            var adapterFilters = new List<AnimeSaturnSearchFilterInput>();

            foreach (var language in _utilities.GetLanguagesArray(filter.Languages))
            {
                adapterFilters.Add(new AnimeSaturnSearchFilterInput()
                {
                    Language = language,
                    Genres = _utilities.GetGenreArray(filter.Genres),
                    Statuses = _utilities.GetStatusesArray(filter.Statuses),
                    Years = filter.Years

                });
            }

            var taskList = new List<Task<AnimeModel[]>>();

            foreach (var adapterFilter in adapterFilters)
            {
                taskList.Add(_adapter.SearchByFilters(adapterFilter, cancellationToken));
            }

            var animeList = new List<AnimeModel>();

            foreach (var task in taskList)
            {
                animeList.AddRange(await task);
            }

            return animeList.ToArray();
        }

        public Task<AnimeModel[]> SearchAnime(string title, CancellationToken cancellationToken = default)
        {
            return _adapter.SearchByTitle(title, cancellationToken);
        }

        public async Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter, CancellationToken cancellationToken = default)
        {
            var animeList = await SearchAnime(filter, cancellationToken);

            return animeList
                .Where(anime => anime.Title.Contains(title))
                .ToArray();
        }
    }
}
