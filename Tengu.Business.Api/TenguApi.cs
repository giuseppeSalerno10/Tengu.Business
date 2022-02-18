using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Core;

namespace Tengu.Business.API
{
    public class TenguApi : ITenguApi
    {
        private readonly IAnimeUnityAdapter _animeUnityAdapter;
        private readonly IAnimeSaturnAdapter _animeSaturnAdapter;
        private readonly IKitsuAdapter _kitsuAdapter;
        private readonly IAnimeUnityUtilities _animeUnityUtilites;
        private readonly IAnimeSaturnUtilities _animeSaturnUtilities;

        public TenguApi(
            IAnimeUnityAdapter animeUnityAdapter, 
            IAnimeSaturnAdapter animeSaturnAdapter, 
            IKitsuAdapter KitsuAdapter,
            IAnimeUnityUtilities animeUnityUtilites,
            IAnimeSaturnUtilities animeSaturnUtilities
            )
        {
            _animeUnityAdapter = animeUnityAdapter;
            _animeSaturnAdapter = animeSaturnAdapter;
            _kitsuAdapter = KitsuAdapter;

            _animeUnityUtilites = animeUnityUtilites;
            _animeSaturnUtilities = animeSaturnUtilities;
        }



        public async Task<AnimeModel[]> SearchAnime(string title) 
        { 
            throw new NotImplementedException();
        }
        public async Task<AnimeModel[]> SearchAnime(SearchFilter filter) 
        {
            throw new NotImplementedException();
        }
        public async Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter) 
        {
            throw new NotImplementedException();
        }

        public async Task<EpisodeModel[]> GetLatestEpisode(int count) 
        {
            throw new NotImplementedException();
        }

        public async Task Download() 
        {
            throw new NotImplementedException();
        }
    }
}
