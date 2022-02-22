using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeUnityUtilities
    {
        Task<AnimeUnityCreateSessionOutput> CreateSession();
        AnimeUnityGenre[] GetGenreArray(IEnumerable<Genres> genres);
        string GetStatus(Statuses status);
    }
}