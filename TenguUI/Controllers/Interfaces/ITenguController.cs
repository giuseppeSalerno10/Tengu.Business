using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;

namespace TenguUI.Controllers.Interfaces
{
    public interface ITenguController
    {
        Task<DownloadMonitor> StartDownloadAsync(string episodeUrl, TenguHosts episodeHost);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, TenguHosts animeHost, int offset = 0, int limit = 0);
        Task<AnimeModel[]> SearchAnimesAsync(string title, TenguSearchFilter searchFilter);
        void SetHosts(TenguHosts[] hosts);
    }
}