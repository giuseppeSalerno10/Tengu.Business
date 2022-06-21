using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.DTO.Output.AnimeUnity;
using Tengu.Business.Core.DTO.Output.AnimeUnity.Object;

namespace Tengu.Business.Core.Utilities.Interfaces
{
    public interface IAnimeUnityUtilities
    {
        Task<AnimeUnityCreateSessionOutput> CreateSession();
        AnimeUnityGenre[] GetGenreArray(IEnumerable<Genres> genres);
        string GetStatus(Statuses status);
    }
}