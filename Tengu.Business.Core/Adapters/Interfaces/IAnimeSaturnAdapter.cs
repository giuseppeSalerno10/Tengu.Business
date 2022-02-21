using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeSaturnAdapter
    {
        Task DownloadAsync(string downloadPath, string animeUrl, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByFiltersAsync(AnimeSaturnSearchFilterInput searchFilter, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByTitleAsync(string title, CancellationToken cancellationToken = default);
    }
}