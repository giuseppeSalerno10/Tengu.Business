using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IKitsuAdapter
    {
        Task<KitsuAnimeModel[]> SearchAnimeAsync(string titles, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> GetUpcomingAnimeAsync(int offset = 0, int limit = 20, CancellationToken cancellationToken = default);

    }
}