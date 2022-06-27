using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;

namespace Tengu.Business.API.Controller.Interfaces
{
    public interface IKitsuController
    {
        Task<OperationResult<KitsuAnimeModel[]>> GetUpcomingAnimeAsync(int offset, int limit, CancellationToken cancellationToken);
        Task<OperationResult<KitsuAnimeModel[]>> SearchAnimeAsync(string title, int offset, int limit, CancellationToken cancellationToken);
    }
}