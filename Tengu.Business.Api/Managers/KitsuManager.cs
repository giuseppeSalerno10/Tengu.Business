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

        public Task<AnimeModel[]> SearchAnime(string title, int limit, CancellationToken cancellationToken = default)
        {
            return _adapter.SearchAnime(title,limit);
        }

        public Task<AnimeModel[]> GetUpcomingAnime(int limit, CancellationToken cancellationToken = default)
        {
            return _adapter.GetUpcomingAnime(limit);
        }
    }
}
