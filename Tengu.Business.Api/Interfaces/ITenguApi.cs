﻿using Downla;
using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;

namespace Tengu.Business.API.Interfaces
{
    public interface ITenguApi
    {
        TenguHosts[] CurrentHosts { get; set; }
        string DownloadPath { get; set; }
        long MaxPacketSize { get; set; }
        int MaxConnections { get; set; }

        event OnDownlaEventDelegate OnStatusChange;
        event OnDownlaEventDelegate OnPacketDownloaded;

        Task<TenguResult<DownloadMonitor>> StartDownloadAsync(string episodeUrl, TenguHosts host, CancellationToken cancellationToken = default);
        Task<TenguResult<Calendar[]>> GetCalendarAsync(CancellationToken cancellationToken = default);
        Task<TenguResult<EpisodeModel[]>> GetEpisodesAsync(string animeId, TenguHosts host, int offset = 0, int count = 0, CancellationToken cancellationToken = default);
        Task<TenguResult<EpisodeModel[]>> GetLatestEpisodeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<TenguResult<KitsuAnimeModel[]>> KitsuSearchAnimeAsync(string title, int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<TenguResult<KitsuAnimeModel[]>> KitsuUpcomingAnimeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default);
        Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(TenguSearchFilter filter, int count = 30, CancellationToken cancellationToken = default);
        Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(string title, int count = 30, CancellationToken cancellationToken = default);
        Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(string title, TenguSearchFilter filter, int count = 30, CancellationToken cancellationToken = default);
    }
}