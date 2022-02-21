using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface ITenguApi
    {
        Hosts[] CurrentHosts { get; set; }
        public string DownloadPath { get; set; }

        Task<KitsuAnimeModel[]> KitsuUpcomingAnime(int offset = 0, int limit = 20, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> KitsuSearchAnime(string title, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);

        Task Download(string episodeId, Hosts host, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodes(string animeId, Hosts host, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisode(int offset, int limit, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(SearchFilter filter, bool kitsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, bool kitsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter, bool kitsuSearch = false, CancellationToken cancellationToken = default);
    }
}