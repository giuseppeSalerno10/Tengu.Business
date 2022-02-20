using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeSaturnAdapter
    {
        Task Download(string downloadPath, string animeUrl, string fileName, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisodes(int offset, int limit, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodes(AnimeModel anime, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByFilters(AnimeSaturnSearchFilterInput searchFilter, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByTitle(string title, CancellationToken cancellationToken = default);
    }
}