using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Commons.Services.Interfaces;

namespace Tengu.Business.API.Controller
{
    public class KitsuController : IKitsuController
    {
        private readonly IKitsuManager _manager;
        private readonly IManipulationService _manipulationService;

        public KitsuController(IKitsuManager manager, IManipulationService manipulationService)
        {
            _manager = manager;
            _manipulationService = manipulationService;
        }

        public async Task<TenguResult<KitsuAnimeModel[]>> GetUpcomingAnimeAsync(int offset, int limit, CancellationToken cancellationToken)
        {
            var result = new TenguResult<KitsuAnimeModel[]>();

            try
            {
                result.Data = await _manager.GetUpcomingAnimeAsync(offset, limit, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }

        public async Task<TenguResult<KitsuAnimeModel[]>> SearchAnimeAsync(string title, int offset, int limit, CancellationToken cancellationToken)
        {
            var result = new TenguResult<KitsuAnimeModel[]>();

            try
            {
                result.Data = await _manager.SearchAnimeAsync(title, offset, limit, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }
    }
}
