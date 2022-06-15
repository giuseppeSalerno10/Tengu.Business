using Tengu.Business.Commons;

namespace Tengu.Business.API.Controller.Interfaces
{
    public interface IKitsuController
    {
        Task<KitsuAnimeModel[]> GetUpcomingAnimeAsync(int offset, int limit, CancellationToken cancellationToken);
        Task<KitsuAnimeModel[]> SearchAnimeAsync(string title, int offset, int limit, CancellationToken cancellationToken);
    }
}