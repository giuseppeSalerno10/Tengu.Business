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
        public Task Download(string downloadPath, string animeUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<EpisodeModel[]> GetLatestEpisode(int count, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AnimeModel[]> SearchByFilters(AnimeSaturnSearchFilterInput searchFilter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AnimeModel[]> SearchByTitle(string title, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
