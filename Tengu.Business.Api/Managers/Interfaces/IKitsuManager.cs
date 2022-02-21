using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface IKitsuManager
    {
        Task<KitsuAnimeModel[]> SearchAnime(string title, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);
        Task<KitsuAnimeModel[]> GetUpcomingAnime(int offset = 0, int limit = 20, CancellationToken cancellationToken = default);
    }
}
