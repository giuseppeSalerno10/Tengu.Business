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

        public Task<KitsuAnimeModel[]> SearchAnime(string title, int offset = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            return _adapter.SearchAnime(title, offset, limit);
        }

        public Task<KitsuAnimeModel[]> GetUpcomingAnime(int offset = 0, int limit = 20, CancellationToken cancellationToken = default)
        {
            return _adapter.GetUpcomingAnime(offset, limit);
        }
    }
}
