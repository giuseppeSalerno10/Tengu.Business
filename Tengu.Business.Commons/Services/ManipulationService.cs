using Tengu.Business.Commons.Objects;
using Tengu.Business.Commons.Services.Interfaces;

namespace Tengu.Business.Commons.Services
{
    public class ManipulationService : IManipulationService
    {
        public void HandleTenguException<TModel>(Exception e, ref OperationResult<TModel> result)
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
