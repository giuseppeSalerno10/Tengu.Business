using Tengu.Business.API.Managers.Interfaces;
using Tengu.Business.Commons.Models;
using Tengu.Business.Core.Adapters.Interfaces;

namespace Tengu.Business.API.Managers
{
    public class KitsuManager : IKitsuManager
    {
        private readonly IKitsuAdapter _adapter;

        public KitsuManager(IKitsuAdapter adapter)
        {
            _adapter = adapter;
        }

        public Task<KitsuAnimeModel[]> SearchAnimeAsync(string title, int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            return _adapter.SearchAnimeAsync(title, offset, limit);
        }

        public Task<KitsuAnimeModel[]> GetUpcomingAnimeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            return _adapter.GetUpcomingAnimeAsync(offset, limit);
        }
    }
}
