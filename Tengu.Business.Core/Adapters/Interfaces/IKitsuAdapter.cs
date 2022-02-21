using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IKitsuAdapter
    {
        Task<KitsuAnimeModel[]> SearchAnime(string titles, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> GetUpcomingAnime(int offset = 0, int limit = 20, CancellationToken cancellationToken = default);

    }
}