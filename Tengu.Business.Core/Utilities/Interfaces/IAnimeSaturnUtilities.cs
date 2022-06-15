using Tengu.Business.Commons;
using Tengu.Business.Core.DTO.Output.AnimeSaturn;

namespace Tengu.Business.Core
{
    public interface IAnimeSaturnUtilities
    {
        Task<AnimeSaturnCreateSessionOutput> CreateSession();
        string[] GetGenreArray(IEnumerable<Genres> genres);
        string GetStatus(Statuses status);
    }
}