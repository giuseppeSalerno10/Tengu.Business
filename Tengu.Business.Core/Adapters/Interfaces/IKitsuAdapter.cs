using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IKitsuAdapter
    {
        Task<AnimeModel[]> SearchAnime(string titles);
    }
}