using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Commons.Services.Interfaces;

namespace Tengu.Business.Commons.Services
{
    public class ManipulationService : IManipulationService
    {
        public void HandleTenguException<TModel>(Exception e, ref TenguResult<TModel> result)
        {
            HandleTenguException(e, ref result);
        }

        public void HandleTenguException(Exception e, ref TenguResult result)
        {
            switch (e)
            {
                case TenguException tenguException:
                    result.Exception = tenguException;
                    break;

                case Exception exception:
                    result.Exception = new TenguException(exception.Message);
                    break;
            }

            result.Success = false;
        }
    }
}
