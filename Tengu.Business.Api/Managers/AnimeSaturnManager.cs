﻿using Downla;
using Downla.Interfaces;
using Downla.Models;
using Downla.Models.M3U8Models;
using Tengu.Business.API.DTO;
using Tengu.Business.API.Managers.Interfaces;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.Adapters.Interfaces;
using Tengu.Business.Core.DTO.Input.AnimeSaturn;
using Tengu.Business.Core.Utilities.Interfaces;

namespace Tengu.Business.API.Managers
{
    public class AnimeSaturnManager : IAnimeSaturnManager
    {
        private readonly IAnimeSaturnAdapter _adapter;
        private readonly IAnimeSaturnUtilities _utilities;
        private readonly IDownlaClient _downlaClient;

        public AnimeSaturnManager(IAnimeSaturnAdapter adapter, IAnimeSaturnUtilities utilities, IDownlaClient downlaClient)
        {
            _adapter = adapter;
            _utilities = utilities;
            _downlaClient = downlaClient;
        }

        public Task<DownloadMonitor> StartDownloadAsync(string episodeUrl, CancellationToken cancellationToken)
        {
            var downloadUrl = _adapter.GetDownloadUrl(episodeUrl);
            Task<DownloadMonitor> downloadTask;

            if (downloadUrl.Contains("m3u8"))
            {
                var urlSplit = downloadUrl.Split("/");
                downloadTask = _downlaClient.StartM3U8DownloadAsync(new Uri(downloadUrl), $"{urlSplit[5]}-{urlSplit[6]}.mp4", 50, ct: cancellationToken);
            }
            else
            {
                Dictionary<string, string> headers = new Dictionary<string, string>()
                {
                    { "Referer", episodeUrl}
                };
                downloadTask = _downlaClient.StartFileDownloadAsync(new Uri(downloadUrl), sleepTime: 100, headers, ct: cancellationToken);
            }

            return downloadTask;
        }

        public Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset, int count, CancellationToken cancellationToken)
        {
            return _adapter.GetEpisodesAsync(animeId, offset, count, cancellationToken);
        }

        public Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken)
        {
            return _adapter.GetLatestEpisodesAsync(offset, limit, cancellationToken);
        }

        public Task<Calendar> GetCalendar(CancellationToken cancellationToken)
        {
            return _adapter.GetCalendar(cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnimeAsync(TenguSearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var adapterFilter = new AnimeSaturnSearchFilterInput()
            {
                Genres = _utilities.GetGenreArray(filter.Genres),
            };

            if (filter.Status != TenguStatuses.None)
            {
                adapterFilter.Status = _utilities.GetStatus(filter.Status);
            }
            if (!string.IsNullOrEmpty(filter.Year))
            {
                adapterFilter.Year = filter.Year;
            }

            return _adapter.SearchByFiltersAsync(adapterFilter, count, cancellationToken);

        }

        public Task<AnimeModel[]> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken)
        {
            return _adapter.SearchByTitleAsync(title, count, cancellationToken);
        }

        public async Task<AnimeModel[]> SearchAnimeAsync(string title, TenguSearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var animeList = await SearchAnimeAsync(filter, count, cancellationToken);

            return animeList
                .Where(anime => anime.Title.Contains(title))
                .ToArray();
        }
    }
}
