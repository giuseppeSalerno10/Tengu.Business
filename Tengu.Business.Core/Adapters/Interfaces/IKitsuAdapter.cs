using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IKitsuAdapter
    {
        AnimeModel SearchAnime(string[] titles);
    }
}