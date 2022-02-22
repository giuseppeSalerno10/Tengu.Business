using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface IKitsuManager
    {
        Task<KitsuAnimeModel[]> SearchAnimeAsync(string title, int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> GetUpcomingAnimeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
    }
}
