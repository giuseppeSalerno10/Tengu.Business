using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IKitsuAdapter
    {
        Task<KitsuAnimeModel[]> SearchAnime(string titles, int limit, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> GetUpcomingAnime(int offset, int limit, CancellationToken cancellationToken = default);

    }
}