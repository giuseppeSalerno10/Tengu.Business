using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface IAnimeSaturnManager
    {
        Task DownloadAsync(string downloadPath, string episodeId, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, CancellationToken cancellationToken = default);
    }
}
