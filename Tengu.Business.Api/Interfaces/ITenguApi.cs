using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface ITenguApi
    {
        Hosts[] CurrentHosts { get; set; }
        public string DownloadPath { get; set; }

        Task Download(EpisodeModel episode, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisode(int count, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(SearchFilter filter, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter, CancellationToken cancellationToken = default);
    }
}