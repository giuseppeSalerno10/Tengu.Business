using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.Commons;

namespace Tengu.Business.API.Controller
{
    public class KitsuController : IKitsuController
    {
        private readonly IKitsuManager _manager;

        public KitsuController(IKitsuManager manager)
        {
            _manager = manager;
        }

        public Task<KitsuAnimeModel[]> GetUpcomingAnimeAsync(int offset, int limit, CancellationToken cancellationToken)
        {
            return _manager.GetUpcomingAnimeAsync(offset, limit, cancellationToken);
        }

        public Task<KitsuAnimeModel[]> SearchAnimeAsync(string title, int offset, int limit, CancellationToken cancellationToken)
        {
            return _manager.SearchAnimeAsync(title, offset, limit, cancellationToken);
        }
    }
}
