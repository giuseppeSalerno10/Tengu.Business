using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface IKitsuManager
    {
        Task<AnimeModel[]> SearchAnime(string title, int limit, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> GetUpcomingAnime(int limit, CancellationToken cancellationToken = default);
    }
}
