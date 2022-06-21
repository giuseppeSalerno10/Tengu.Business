using Tengu.Business.Commons.Objects;

namespace Tengu.Business.Commons.Services.Interfaces
{
    public interface IManipulationService
    {
        void HandleTenguException<TModel>(Exception e, ref TenguResult<TModel> result);
        void HandleTenguException(Exception e, ref TenguResult result);

    }
}