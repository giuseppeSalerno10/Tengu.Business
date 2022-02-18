using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeUnityAdapter
    {
        void Download(string downloadPath, Uri uri);
        IEnumerable<AnimeModel> SearchByFilters();
        IEnumerable<AnimeModel> SearchByTitle(string title);
    }
}