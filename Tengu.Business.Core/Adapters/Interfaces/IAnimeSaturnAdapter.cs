using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeSaturnAdapter
    {
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByFiltersAsync(AnimeSaturnSearchFilterInput searchFilter, int count = 30, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByTitleAsync(string title, int count = 30, CancellationToken cancellationToken = default);
        Task<string> GetDownloadUrl(string episodeStreamUrl, CancellationToken cancellationToken = default);

    }
}