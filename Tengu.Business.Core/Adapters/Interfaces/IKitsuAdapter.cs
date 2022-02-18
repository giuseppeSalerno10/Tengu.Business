using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IKitsuAdapter
    {
        Task<AnimeModel[]> SearchAnime(string titles, int limit, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> GetUpcomingAnime(int limit, CancellationToken cancellationToken = default);

    }
}