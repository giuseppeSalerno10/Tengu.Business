using Downla;
using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface ITenguApi
    {
        Hosts[] CurrentHosts { get; set; }
        public string DownloadPath { get; set; }

        Task<KitsuAnimeModel[]> KitsuUpcomingAnimeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> KitsuSearchAnimeAsync(string title, int offset = 0, int limit = 30, CancellationToken cancellationToken = default);

        DownloadInfosModel DownloadAsync(string episodeId, Hosts host, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, Hosts host, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisodeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count = 30, bool kitsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, int count = 30, bool kitsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count = 30, bool kitsuSearch = false, CancellationToken cancellationToken = default);
        Task<Calendar[]> GetCalendar(CancellationToken cancellationToken = default);
    }
}