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
        public static KitsuConfig Kitsu { get; } = new KitsuConfig();
    }

    public class KitsuConfig
    {
        public string BaseUrl { get; } = "https://kitsu.io/api/edge/anime?";

        public int MaxAnimes { get; } = 20;
    }

    public class AnimeSaturnConfig
    {
        public string BaseUrl { get; } = "https://www.animesaturn.it";
        public string BaseAnimeUrl { get; } = "https://www.animesaturn.it/anime";
        public string BaseLatestEpisodeUrl { get; } = "https://www.animesaturn.it/fetch_pages.php?request=episodes";
        public string SearchByTitleUrl { get; } = "https://www.animesaturn.it/animelist?";
        public string SearchByFilterUrl { get; } = "https://www.animesaturn.it/filter?";

    }

    public class AnimeUnityConfig
    {
        public string BaseUrl { get; } = "https://www.animeunity.tv";
        public string BaseAnimeUrl { get; } = "https://www.animeunity.tv/anime";
        public string SearchUrl { get; } = "https://www.animeunity.tv/archivio/get-animes";
    }
}
