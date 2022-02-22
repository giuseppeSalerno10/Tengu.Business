using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeSaturnUtilities
    {
        string[] GetGenreArray(IEnumerable<Genres> genres);
        string GetStatus(Statuses status);
    }
}