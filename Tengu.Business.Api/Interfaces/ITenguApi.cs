using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface ITenguApi
    {
        Hosts[] CurrentHosts { get; set; }
        public string DownloadPath { get; set; }

        Task<KitsuAnimeModel[]> KitsuUpcomingAnime(int offset, int limit, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> KitsuSearchAnime(string title, int count, CancellationToken cancellationToken = default);

        Task Download(string episodeId, Hosts host, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisode(int offset, int limit, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(SearchFilter filter, bool kitsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, bool kitsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter, bool kitsuSearch = false, CancellationToken cancellationToken = default);
    }
}