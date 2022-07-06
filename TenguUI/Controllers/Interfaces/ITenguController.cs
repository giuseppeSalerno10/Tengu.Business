using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;

namespace TenguUI.Controllers.Interfaces
{
    public interface ITenguController
    {
        DownloadMonitor DownloadAsync(string episodeUrl, TenguHosts episodeHost);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, TenguHosts animeHost);
        Task<AnimeModel[]> SearchAnimesAsync(string title, TenguSearchFilter searchFilter);
        void SetHosts(TenguHosts[] hosts);
    }
}