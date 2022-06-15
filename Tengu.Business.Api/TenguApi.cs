using Downla;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Tengu.Business.API.Controller;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.Commons;

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


        public async Task<KitsuAnimeModel[]> KitsuUpcomingAnimeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            return await _kitsuController.GetUpcomingAnimeAsync(offset, limit, cancellationToken);
        }
        public async Task<KitsuAnimeModel[]> KitsuSearchAnimeAsync(string title, int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            return await _kitsuController.SearchAnimeAsync(title, offset, limit, cancellationToken);
        }

        public async Task<AnimeModel[]> SearchAnimeAsync(string title, int count = 30, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var animeList = new ConcurrentBag<AnimeModel>();
            var searchTasks = new List<Task<AnimeModel[]>>();
            
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

            return await ElaborateSearch(searchTasks, cancellationToken);
        }
        public async Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count = 30, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var animeList = new ConcurrentBag<AnimeModel>();
            var searchTasks = new List<Task<AnimeModel[]>>();

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

            return await ElaborateSearch(searchTasks, cancellationToken);
        }
        public async Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count = 30, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var searchTasks = new List<Task<AnimeModel[]>>();

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
            return await ElaborateSearch(searchTasks, cancellationToken);
        }

        public async Task<EpisodeModel[]> GetEpisodesAsync(string animeId, Hosts host, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            CheckForHost();
            Task<EpisodeModel[]> getEpisodesTask;

            switch (host)
            {
                case Hosts.AnimeSaturn:
                    getEpisodesTask = _animeSaturnController.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
                    break;

                case Hosts.AnimeUnity:
                    getEpisodesTask = _animeUnityController.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
                    break;

                default:
                    throw new TenguException("No host found");
            }

            EpisodeModel[] episodes;

            try
            {
                episodes = await getEpisodesTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEpisodes");
                throw;
            }

             return episodes;
        }

        public async Task<EpisodeModel[]> GetLatestEpisodeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var episodeList = new List<EpisodeModel>();
            var searchTasks = new List<Task<EpisodeModel[]>>();

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

            foreach (var task in searchTasks)
            {
                try
                {
                    var result = await task;
                    episodeList.AddRange(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in GetLatestEpisode");
                    throw;
                }
            }

            return episodeList.ToArray();
        }

        public async Task<Calendar[]> GetCalendar(CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var calendarList = new List<Calendar>();
            var tasks = new List<Task<Calendar>>();

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

            foreach (var task in tasks)
            {
                try
                {
                    var result = await task;
                    calendarList.Add(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in GetCalendar");
                    throw;
                }
            }

            return calendarList.ToArray();
        }

        public DownloadInfosModel DownloadAsync(string episodeUrl, Hosts host, CancellationToken cancellationToken = default)
        {
            DownloadInfosModel downloadInfo;

            switch (host)
            {
                case Hosts.AnimeSaturn:
                    downloadInfo = _animeSaturnController.DownloadAsync(DownloadPath, episodeUrl, cancellationToken);
                    break;

                case Hosts.AnimeUnity:
                    downloadInfo = _animeUnityController.DownloadAsync(DownloadPath, episodeUrl, cancellationToken);
                    break;

                default:
                    throw new TenguException("No host found");
            }

            return downloadInfo;
        }

        #region Private Methods
        private void CheckForHost()
        {
            if (CurrentHosts.Length == 0) { throw new TenguException("No host defined"); }
        }
        private async Task<AnimeModel[]> ElaborateSearch(List<Task<AnimeModel[]>> searchTasks, CancellationToken cancellationToken)
        {
            var animeList = new List<AnimeModel>();

            foreach (var task in searchTasks)
            {
                foreach (var anime in await task)
                {
                    try
                    {
                        animeList.Add(anime);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in SearchAnime -> ElaborateSearch");
                        throw;
                    }
                }
            }

            return animeList.ToArray();
        }
        #endregion

    }
}
