using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public class AnimeUnityAdapter : IAnimeUnityAdapter
    {
        public Task DownloadAsync(string downloadPath, string animeUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AnimeModel[]> SearchByFiltersAsync(AnimeSaturnSearchFilterInput searchFilter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AnimeModel[]> SearchByTitleAsync(string title, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
