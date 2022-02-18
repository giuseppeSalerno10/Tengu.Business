using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface IKitsuManager
    {
        Task<AnimeModel[]> SearchAnime(string title, CancellationToken cancellationToken = default);
    }
}
