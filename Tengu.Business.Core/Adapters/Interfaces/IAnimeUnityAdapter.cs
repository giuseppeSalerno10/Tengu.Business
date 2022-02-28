using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeUnityAdapter
    {
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAsync(AnimeUnitySearchInput searchFilter, int count = 30, CancellationToken cancellationToken = default);
        Task<Calendar> GetCalendar(CancellationToken cancellationToken = default);
    }
}