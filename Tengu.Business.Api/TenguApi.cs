using Downla;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.API.DTO;
using Tengu.Business.API.Interfaces;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;

namespace Tengu.Business.API
{
    public class TenguApi : ITenguApi
    {
        public Hosts[] CurrentHosts { get; set; } = Array.Empty<Hosts>();
        public string DownloadPath { get; set; } = $"{Environment.CurrentDirectory}\\DownloadedAnime";

        private readonly ILogger<TenguApi> _logger;

        private readonly IAnimeUnityController _animeUnityController;
        private readonly IAnimeSaturnController _animeSaturnController;
        private readonly IKitsuController _kitsuController;

        public TenguApi(
            ILogger<TenguApi> logger, IAnimeUnityController animeUnityController, IAnimeSaturnController animeSaturnController, IKitsuController kitsuController)
        {
            _animeUnityController = animeUnityController;
            _animeSaturnController = animeSaturnController;
            _kitsuController = kitsuController;

            _logger = logger;

            _logger.LogInformation("TenguApi is READY", new { Infos = "NONE" });
        }


        public async Task<TenguResult<KitsuAnimeModel[]>> KitsuUpcomingAnimeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var result = await _kitsuController.GetUpcomingAnimeAsync(offset, limit, cancellationToken);
            return new TenguResult<KitsuAnimeModel[]>()
            {
                Data = result.Data,
                Infos = new TenguResultInfo[] 
                {
                    new TenguResultInfo ()
                    {
                        Exception = result.Exception,
                        Host = result.Host,
                        Success = result.Success
                    }
                }
            };

        }
        public async Task<TenguResult<KitsuAnimeModel[]>> KitsuSearchAnimeAsync(string title, int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var result = await _kitsuController.SearchAnimeAsync(title, offset, limit, cancellationToken);
            return new TenguResult<KitsuAnimeModel[]>()
            {
                Data = result.Data,
                Infos = new TenguResultInfo[]
                {
                    new TenguResultInfo ()
                    {
                        Exception = result.Exception,
                        Host = result.Host,
                        Success = result.Success
                    }
                }
            };
        }

        public Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(string title, int count = 30, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var searchTasks = new List<Task<OperationResult<AnimeModel[]>>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnController.SearchAnimeAsync(title, count, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityController.SearchAnimeAsync(title, count, cancellationToken));
                        break;
                }
            }

            return ElaborateSearch(searchTasks, cancellationToken);
        }
        public Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(SearchFilter filter, int count = 30, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var searchTasks = new List<Task<OperationResult<AnimeModel[]>>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnController.SearchAnimeAsync(filter, count, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityController.SearchAnimeAsync(filter, count, cancellationToken));
                        break;
                }
            }

            return ElaborateSearch(searchTasks, cancellationToken);
        }
        public Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(string title, SearchFilter filter, int count = 30, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var searchTasks = new List<Task<OperationResult<AnimeModel[]>>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnController.SearchAnimeAsync(title, filter, count, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityController.SearchAnimeAsync(title, filter, count, cancellationToken));
                        break;
                }
            }

            return ElaborateSearch(searchTasks, cancellationToken);
        }

        public async Task<TenguResult<EpisodeModel[]>> GetEpisodesAsync(string animeId, Hosts host, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var result = host switch
            {
                Hosts.AnimeSaturn => await _animeSaturnController.GetEpisodesAsync(animeId, offset, limit, cancellationToken),
                Hosts.AnimeUnity => await _animeUnityController.GetEpisodesAsync(animeId, offset, limit, cancellationToken),
                _ => throw new TenguException("No host found")
            };

            return new TenguResult<EpisodeModel[]>()
            {
                Data = result.Data,
                Infos = new TenguResultInfo[] 
                {
                    new TenguResultInfo ()
                    {
                        Exception = result.Exception,
                        Host = result.Host,
                        Success = result.Success
                    }
                }
            };

        }

        public async Task<TenguResult<EpisodeModel[]>> GetLatestEpisodeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var searchTasks = new List<Task<OperationResult<EpisodeModel[]>>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnController.GetLatestEpisodesAsync(offset, limit, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityController.GetLatestEpisodesAsync(offset, limit, cancellationToken));
                        break;
                }
            }

            var episodeList = new List<EpisodeModel>();
            var tenguResults = new List<TenguResultInfo>();

            foreach (var task in searchTasks)
            {
                var result = await task;
                episodeList.AddRange(result.Data);

                tenguResults.Add(new TenguResultInfo()
                {
                    Exception = result.Exception,
                    Host = result.Host,
                    Success = result.Success
                });
            }

            return new TenguResult<EpisodeModel[]>()
            {
                Data = episodeList.ToArray(),
                Infos = tenguResults.ToArray()
            };
        }

        public async Task<TenguResult<Calendar[]>> GetCalendar(CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var tasks = new List<Task<OperationResult<Calendar>>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        tasks.Add(_animeSaturnController.GetCalendar(cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        tasks.Add(_animeUnityController.GetCalendar(cancellationToken));
                        break;
                }
            }

            var calendarList = new List<Calendar>();
            var tenguResults = new List<TenguResultInfo>();

            foreach (var task in tasks)
            {
                var result = await task;
                calendarList.Add(result.Data);
                tenguResults.Add(new TenguResultInfo
                {
                    Exception = result.Exception,
                    Host = result.Host,
                    Success = result.Success
                });
            }

            return new TenguResult<Calendar[]>()
            {
                Data = calendarList.ToArray(),
                Infos = tenguResults.ToArray()
            };

        }

        public TenguResult<DownloadInfosModel> DownloadAsync(string episodeUrl, Hosts host, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var result = host switch
            {
                Hosts.AnimeSaturn => _animeSaturnController.DownloadAsync(DownloadPath, episodeUrl, cancellationToken),
                Hosts.AnimeUnity => _animeUnityController.DownloadAsync(DownloadPath, episodeUrl, cancellationToken),
                _ => throw new TenguException("No host found")
            };

            return new TenguResult<DownloadInfosModel>()
            {
                Data = result.Data,
                Infos = new TenguResultInfo[]
                {
                    new TenguResultInfo ()
                    {
                        Exception = result.Exception,
                        Host = result.Host,
                        Success = result.Success
                    }
                }
            };

        }

        #region Private Methods
        private void CheckForHost()
        {
            if (CurrentHosts.Length == 0) { throw new TenguException("No host defined"); }
        }
        private async Task<TenguResult<AnimeModel[]>> ElaborateSearch(List<Task<OperationResult<AnimeModel[]>>> searchTasks, CancellationToken cancellationToken)
        {
            var animeList = new List<AnimeModel>();
            var tenguResults = new List<TenguResultInfo>();
            foreach (var task in searchTasks)
            {
                var result = await task;
                animeList.AddRange(result.Data);
                tenguResults.Add(new TenguResultInfo()
                {
                    Exception = result.Exception,
                    Host = result.Host,
                    Success = result.Success
                });
            }

            return new() { Data = animeList.ToArray(), Infos = tenguResults.ToArray() };
        }
        #endregion

    }
}
