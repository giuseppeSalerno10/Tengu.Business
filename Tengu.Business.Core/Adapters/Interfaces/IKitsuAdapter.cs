using Tengu.Business.Commons.Models;

namespace Tengu.Business.Core.Adapters.Interfaces
{
    public interface IKitsuAdapter
    {
        Task<KitsuAnimeModel[]> SearchAnimeAsync(string titles, int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> GetUpcomingAnimeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);

    }
}