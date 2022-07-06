﻿using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;

namespace TenguUI.Managers.Interfaces
{
    public interface ITenguManager
    {
        Task<AnimeModel[]> SearchAnimesAsync(string title, TenguSearchFilter filter);
        Task<AnimeModel[]> SearchAnimesAsync(string title);
        Task<AnimeModel[]> SearchAnimesAsync(TenguSearchFilter filter);
        void SetHosts(TenguHosts[] hosts);
        DownloadMonitor DownloadAsync(string episodeUrl, TenguHosts episodeHost);
        Task<EpisodeModel[]> GetEpisodesAsync(string animeId, TenguHosts animeHost, int offset, int limit);
    }
}