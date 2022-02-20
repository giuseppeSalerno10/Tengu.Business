using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface IKitsuManager
    {
        Task<KitsuAnimeModel[]> SearchAnime(string title, int limit, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> GetUpcomingAnime(int offset, int limit, CancellationToken cancellationToken = default);
    }
}
