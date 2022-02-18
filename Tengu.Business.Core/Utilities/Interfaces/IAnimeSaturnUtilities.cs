using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeSaturnUtilities
    {
        Task<AnimeModel[]> FillAnimeList(IEnumerable<AnimeModel> animeList, CancellationToken cancellationToken = default);
        string[] GetGenreArray(IEnumerable<Genres> genres);
        string[] GetLanguagesArray(IEnumerable<Languages> languages);
        string[] GetStatusesArray(IEnumerable<Statuses> statuses);
    }
}