using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Core;

using Microsoft.Extensions.Logging;

namespace Tengu.Business.API
{
    public class TenguApi : ITenguApi
    {
        public Hosts[] CurrentHosts { get; set; } = Array.Empty<Hosts>();
        public string DownloadPath { get; set; } = $"{Environment.CurrentDirectory}\\DownloadedAnime";

        private readonly ILogger<TenguApi> _logger;

        private readonly IAnimeUnityManager _animeUnityManager;
        private readonly IAnimeSaturnManager _animeSaturnManager;
        private readonly IKitsuManager _kitsuManager;

        public TenguApi(
            IAnimeUnityManager animeUnityManager, 
            IAnimeSaturnManager animeSaturnManager, 
            IKitsuManager kitsuManager, 
            ILogger<TenguApi> logger)
        {
            _animeUnityManager = animeUnityManager;
            _animeSaturnManager = animeSaturnManager;
            _kitsuManager = kitsuManager;

            _logger = logger;
        }


        public async Task<KitsuAnimeModel[]> KitsuUpcomingAnime(int offset, int limit, CancellationToken cancellationToken = default)
        {
            return await _kitsuManager.GetUpcomingAnime(offset, limit, cancellationToken);
        }
        public async Task<KitsuAnimeModel[]> KitsuSearchAnime(string title, int count, CancellationToken cancellationToken = default)
        {
            return await _kitsuManager.SearchAnime(title, count, cancellationToken);
        }


        public async Task<AnimeModel[]> SearchAnime(string title, bool kitsuSearch = false, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var animeList = new ConcurrentBag<AnimeModel>();
            var searchTasks = new List<Task<AnimeModel[]>>();
            
            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.SearchAnime(title, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.SearchAnime(title, cancellationToken));
                        break;
                }
            }

            return await ElaborateSearch(searchTasks, kitsuSearch, cancellationToken);
        }
        public async Task<AnimeModel[]> SearchAnime(SearchFilter filter, bool kitsuSearch = false, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var animeList = new ConcurrentBag<AnimeModel>();
            var searchTasks = new List<Task<AnimeModel[]>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.SearchAnime(filter, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.SearchAnime(filter, cancellationToken));
                        break;
                }
            }

            return await ElaborateSearch(searchTasks, kitsuSearch, cancellationToken);
        }
        public async Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter, bool kitsuSearch = false, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var searchTasks = new List<Task<AnimeModel[]>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.SearchAnime(title, filter, cancellationToken));
                        break;
                    case Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.SearchAnime(title, filter, cancellationToken));
                        break;
                }
            }
            return await ElaborateSearch(searchTasks, kitsuSearch, cancellationToken);
        }

        public async Task<EpisodeModel[]> GetEpisodes(string animeId, Hosts host, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            CheckForHost();
            Task<EpisodeModel[]> getEpisodesTask;

            switch (host)
            {
                case Hosts.AnimeSaturn:
                    getEpisodesTask = _animeSaturnManager.GetEpisodes(animeId, offset, limit, cancellationToken);
                    break;

                case Hosts.AnimeUnity:
                    getEpisodesTask = _animeUnityManager.GetEpisodes(animeId, offset, limit, cancellationToken);
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
                _logger.LogError(ex, "Error in GetEpisodes", new { animeId, host });
                throw;
            }

             return episodes;
        }

        public async Task<EpisodeModel[]> GetLatestEpisode(int offset, int limit, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var episodeList = new List<EpisodeModel>();
            var searchTasks = new List<Task<EpisodeModel[]>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Commons.Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.GetLatestEpisodes(offset, limit, cancellationToken));
                        break;
                    case Commons.Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.GetLatestEpisodes(offset, limit, cancellationToken));
                        break;
                }
            }

            foreach (var task in searchTasks)
            {
                try
                {
                    episodeList.AddRange(await task);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in GetLatestEpisode", limit);
                    throw;
                }
            }

            return episodeList.ToArray();
        }


        public async Task Download(string episodeId, Hosts host, CancellationToken cancellationToken = default)
        {
            Task task;
            switch (host)
            {
                case Hosts.AnimeSaturn:
                    task = _animeSaturnManager.Download(DownloadPath, episodeId, cancellationToken);
                    break;

                case Hosts.AnimeUnity:
                    task = _animeUnityManager.Download(DownloadPath, episodeId, cancellationToken);
                    break;

                default:
                    throw new TenguException("No host found");
            }

            try
            {
                await task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Download", new { episodeId, host });
                throw;
            }
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
                        _logger.LogError(ex, message:"Error in SearchAnime -> ElaborateSearch", new { searchTasks, kitsuSearch} );
                        throw;
                    }
                }
            }

            if (kitsuSearch)
            {
                Parallel.ForEach(animeList, async (anime) =>
                {
                    var titleSearch = _kitsuManager.SearchAnime(anime.Title, 1, cancellationToken);

                    var titleResponse = (await titleSearch)[0];

                    anime.KitsuAttributes = titleResponse;
                });
            }

            return animeList.ToArray();
        }
        #endregion

    }
}
