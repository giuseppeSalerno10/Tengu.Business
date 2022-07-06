using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.DTO.Output.AnimeSaturn;

namespace Tengu.Business.Core.Utilities.Interfaces
{
    public interface IAnimeSaturnUtilities
    {
        Task<AnimeSaturnCreateSessionOutput> CreateSession();
        string[] GetGenreArray(IEnumerable<TenguGenres> genres);
        string GetStatus(TenguStatuses status);
    }
}