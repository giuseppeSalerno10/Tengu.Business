using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Core;

namespace Tengu.Business.API
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
