namespace Tengu.Business.Commons
{
    public static class Config
    {
        public static AnimeSaturnConfig AnimeSaturn { get; } = new AnimeSaturnConfig();
        public static AnimeUnityConfig AnimeUnity { get; } = new AnimeUnityConfig();
        public static KitsuConfig Kitsu { get; } = new KitsuConfig();

        public static CommonConfig Common { get; } = new CommonConfig();
    }

    public class CommonConfig
    {
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36 OPR/83.0.4254.46";
        public long PacketSize { get; set; } = 2621440;
        public int Connections { get; set; } = 10;
    }

    public class KitsuConfig
    {
        public string BaseUrl { get; } = "https://kitsu.io/api/edge/anime?";

        public int MaxAnimes { get; } = 20;
    }

    public class AnimeSaturnConfig
    {
        public string BaseUrl { get; } = "https://www.animesaturn.cc";
        public string BaseAnimeUrl { get; } = $"https://www.animesaturn.cc/anime";
        public string BaseLatestEpisodeUrl { get; } = "https://www.animesaturn.cc/fetch_pages.php?request=episodes";
        public string SearchByTitleUrl { get; } = "https://www.animesaturn.cc/animelist?";
        public string SearchByFilterUrl { get; } = "https://www.animesaturn.cc/filter?";
        public string CalendarUrl { get; } = "https://www.animesaturn.cc/calendario";
    }

    public class AnimeUnityConfig
    {
        public string BaseUrl { get; } = "https://www.animeunity.tv";
        public string BaseAnimeUrl { get; } = "https://www.animeunity.tv/anime";
        public string BaseEpisodeUrl { get; } = "https://www.animeunity.tv/info_api";
        public string SearchUrl { get; } = "https://www.animeunity.tv/archivio/get-animes";
        public object CalendarUrl { get; } = "https://www.animeunity.tv/calendario";
    }
}
