using Tengu.Business.Commons;
using Tengu.Business.Commons.Objects;

namespace Tengu.Business.API.Controller.Interfaces
{
    public interface IKitsuController
    {
        Task<TenguResult<KitsuAnimeModel[]>> GetUpcomingAnimeAsync(int offset, int limit, CancellationToken cancellationToken);
        Task<TenguResult<KitsuAnimeModel[]>> SearchAnimeAsync(string title, int offset, int limit, CancellationToken cancellationToken);
    }
}