using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Commons
{
    public static class Config
    {
        public static AnimeSaturnConfig AnimeSaturn { get; } = new AnimeSaturnConfig();
        public static AnimeUnityConfig AnimeUnity { get; } = new AnimeUnityConfig();
    }

    public class AnimeSaturnConfig
    {
        public string BaseUrl { get; } = "https://www.animesaturn.it/";
        public string BaseAnimeUrl { get; } = "https://www.animesaturn.it/anime/";
        public string BaseEpisodeUrl { get; } = "https://www.animesaturn.it/ep/";
        public string BaseLatestEpisodeUrl { get; } = "https://www.animesaturn.it/fetch_pages.php?request=episodes";
        public string SearchByTitleUrl { get; } = "https://www.animesaturn.it/index.php?";
        public string SearchByFilterUrl { get; } = "https://www.animesaturn.it/filter?";

    }

    public class AnimeUnityConfig
    {
        public string BaseUrl { get; } = "https://www.animeunity.tv";
        public string SearchByTitleUrl { get; } = "https://www.animeunity.tv/livesearch";
        public string SearchByFilterUrl { get; } = "https://www.animeunity.tv/archivio/get-animes";

    }
}
