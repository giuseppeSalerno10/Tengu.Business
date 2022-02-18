using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeSaturnAdapter
    {
        Task Download(string downloadPath, string animeUrl, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisodeAsync(int count, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByFiltersAsync(AnimeSaturnSearchFilterInput searchFilter, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByTitleAsync(string title, CancellationToken cancellationToken = default);
    }
}