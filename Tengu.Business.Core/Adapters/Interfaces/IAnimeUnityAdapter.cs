using Tengu.Business.Commons.Models;
using Tengu.Business.Core.DTO.Input.AnimeUnity;

namespace Tengu.Business.Core.Adapters.Interfaces
{
    public interface IAnimeUnityAdapter
    {
        Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAsync(AnimeUnitySearchInput searchFilter, int count = 30, CancellationToken cancellationToken = default);
        Task<Calendar> GetCalendar(CancellationToken cancellationToken = default);
        Task<string> GetStreamUrl(string episodeUrl, CancellationToken cancellationToken = default);
    }
}