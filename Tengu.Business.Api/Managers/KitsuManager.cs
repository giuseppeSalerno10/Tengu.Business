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

        public Task<AnimeModel[]> SearchAnime(string title, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}
