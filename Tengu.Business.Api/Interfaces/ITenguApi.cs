using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface ITenguApi
    {
        Hosts[] CurrentHosts { get; set; }
        public string DownloadPath { get; set; }

        Task<KitsuAnimeModel[]> KitsuUpcomingAnimeAsync(int offset = 0, int limit = 20, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> KitsuSearchAnimeAsync(string title, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);

        Task DownloadAsync(string episodeId, Hosts host, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, Hosts host, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisodeAsync(int offset, int limit, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, bool kitsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, bool kitsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, bool kitsuSearch = false, CancellationToken cancellationToken = default);
    }
}