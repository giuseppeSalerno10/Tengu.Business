using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface ITenguApi
    {
        Hosts[] CurrentHosts { get; set; }
        public string DownloadPath { get; set; }

        Task<AnimeModel[]> KitsuUpcomingAnime(int count, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> KitsuSearchAnime(string title, int count, CancellationToken cancellationToken = default);

        Task Download(EpisodeModel episode, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisode(int count, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(SearchFilter filter, bool kintsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, bool kintsuSearch = false, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter, bool kintsuSearch = false, CancellationToken cancellationToken = default);
    }
}