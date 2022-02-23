using Downla;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public class TenguApi : ITenguApi
    {
        public Hosts[] CurrentHosts { get; set; } = Array.Empty<Hosts>();
        public string DownloadPath { get; set; } = $"{Environment.CurrentDirectory}\\DownloadedAnime";

        private readonly ILogger _logger;

        private readonly IAnimeUnityManager _animeUnityManager;
        private readonly IAnimeSaturnManager _animeSaturnManager;
        private readonly IKitsuManager _kitsuManager;

        public TenguApi(
            IAnimeUnityManager animeUnityManager, 
            IAnimeSaturnManager animeSaturnManager, 
            IKitsuManager kitsuManager, 
            ILogger logger)
        {
            _animeUnityManager = animeUnityManager;
            _animeSaturnManager = animeSaturnManager;
            _kitsuManager = kitsuManager;

            _logger = logger;

            _logger.WriteInfo("TenguApi is READY", new { Infos = "NONE" });
        }


        public async Task<KitsuAnimeModel[]> KitsuUpcomingAnimeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            return await _kitsuManager.GetUpcomingAnimeAsync(offset, limit, cancellationToken);
        }
        public async Task<KitsuAnimeModel[]> KitsuSearchAnimeAsync(string title, int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            return await _kitsuManager.SearchAnimeAsync(title, offset, limit, cancellationToken);
        }


        public async Task<AnimeModel[]> SearchAnimeAsync(string title, int count = 30, bool kitsuSearch = false, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var animeList = new ConcurrentBag<AnimeModel>();
            var searchTasks = new List<Task<AnimeModel[]>>();
            
            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.SearchAnimeAsync(title, count, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.SearchAnimeAsync(title, count, cancellationToken));
                        break;
                }
            }

            return await ElaborateSearch(searchTasks, kitsuSearch, cancellationToken);
        }
        public async Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count = 30, bool kitsuSearch = false, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var animeList = new ConcurrentBag<AnimeModel>();
            var searchTasks = new List<Task<AnimeModel[]>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.SearchAnimeAsync(filter, count, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.SearchAnimeAsync(filter, count, cancellationToken));
                        break;
                }
            }

            return await ElaborateSearch(searchTasks, kitsuSearch, cancellationToken);
        }
        public async Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count = 30, bool kitsuSearch = false, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var searchTasks = new List<Task<AnimeModel[]>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.SearchAnimeAsync(title, filter, count, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.SearchAnimeAsync(title, filter, count, cancellationToken));
                        break;
                }
            }
            return await ElaborateSearch(searchTasks, kitsuSearch, cancellationToken);
        }

        public async Task<EpisodeModel[]> GetEpisodesAsync(string animeId, Hosts host, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            CheckForHost();
            Task<EpisodeModel[]> getEpisodesTask;

            switch (host)
            {
                case Hosts.AnimeSaturn:
                    getEpisodesTask = _animeSaturnManager.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
                    break;

                case Hosts.AnimeUnity:
                    getEpisodesTask = _animeUnityManager.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
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
                _logger.WriteError("Error in GetEpisodes", ex);
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
                        searchTasks.Add(_animeSaturnManager.GetLatestEpisodesAsync(offset, limit, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.GetLatestEpisodesAsync(offset, limit, cancellationToken));
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
                    _logger.WriteError("Error in GetLatestEpisode", ex);
                    throw;
                }
            }

            return episodeList.ToArray();
        }

        public DownloadInfosModel DownloadAsync(string episodeUrl, Hosts host, CancellationToken cancellationToken = default)
        {
            DownloadInfosModel downloadInfo;

            switch (host)
            {
                case Hosts.AnimeSaturn:
                    downloadInfo = _animeSaturnManager.DownloadAsync(DownloadPath, episodeUrl, cancellationToken);
                    break;

                case Hosts.AnimeUnity:
                    downloadInfo = _animeUnityManager.DownloadAsync(DownloadPath, episodeUrl, cancellationToken);
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
        private async Task<AnimeModel[]> ElaborateSearch(List<Task<AnimeModel[]>> searchTasks, bool kitsuSearch, CancellationToken cancellationToken)
        {
            var animeList = new ConcurrentBag<AnimeModel>();

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
                        _logger.WriteError(message:"Error in SearchAnime -> ElaborateSearch", ex);
                        throw;
                    }
                }
            }

            if (kitsuSearch)
            {
                Parallel.ForEach(animeList, async (anime) =>
                {
                    var titleSearch = _kitsuManager.SearchAnimeAsync(anime.Title, 0, 1, cancellationToken);

                    var titleResponse = (await titleSearch)[0];

                    anime.KitsuAttributes = titleResponse;
                });
            }

            return animeList.ToArray();
        }
        #endregion

    }
}
