using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeSaturnUtilities
    {
        string[] GetGenreArray(IEnumerable<Genres> genres);
        string[] GetLanguagesArray(IEnumerable<Languages> languages);
        string[] GetStatusesArray(IEnumerable<Statuses> statuses);
    }
}