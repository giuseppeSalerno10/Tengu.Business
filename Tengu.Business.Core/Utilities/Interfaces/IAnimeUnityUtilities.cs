using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.DTO.Output.AnimeUnity;
using Tengu.Business.Core.DTO.Output.AnimeUnity.Object;

namespace Tengu.Business.Core.Utilities.Interfaces
{
    public interface IAnimeUnityUtilities
    {
        Task<AnimeUnityCreateSessionOutput> CreateSession();
        Task<string> GetDownloadUrl(string scwsId, string fileName);
        AnimeUnityGenre[] GetGenreArray(IEnumerable<TenguGenres> genres);
        string GetStatus(TenguStatuses status);
    }
}