using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Core;

namespace Tengu.Business.API
{
    public class TenguApi : ITenguApi
    {
        public Hosts[] CurrentHosts { get; set; } = Array.Empty<Hosts>();
        public string DownloadPath { get; set; } = $"{Environment.CurrentDirectory}\\DownloadedAnime";


        private readonly IAnimeUnityManager _animeUnityManager;
        private readonly IAnimeSaturnManager _animeSaturnManager;
        private readonly IKitsuManager _kitsuManager;

        public TenguApi(IAnimeUnityManager animeUnityManager, IAnimeSaturnManager animeSaturnManager, IKitsuManager kitsuManager)
        {
            _animeUnityManager = animeUnityManager;
            _animeSaturnManager = animeSaturnManager;
            _kitsuManager = kitsuManager;
        }

        public async Task<AnimeModel[]> SearchAnime(string title, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var animeList = new List<AnimeModel>();
            var searchTasks = new List<Task<AnimeModel[]>>();
            
            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Commons.Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.SearchAnime(title, cancellationToken));
                        break;
                    case Commons.Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.SearchAnime(title, cancellationToken));
                        break;
                }
            }

            foreach (var task in searchTasks)
            {
                try
                {
                    animeList.AddRange(await task);
                }
                catch (Exception ex)
                {
                    //Logging
                }
            }

            return animeList.ToArray();
        }
        public async Task<AnimeModel[]> SearchAnime(SearchFilter filter, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var animeList = new List<AnimeModel>();
            var searchTasks = new List<Task<AnimeModel[]>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Commons.Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.SearchAnime(filter, cancellationToken));
                        break;
                    case Commons.Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.SearchAnime(filter, cancellationToken));
                        break;
                }
            }

            foreach (var task in searchTasks)
            {
                try
                {
                    animeList.AddRange(await task);
                }
                catch (Exception ex)
                {
                    // Logging
                }
            }

            return animeList.ToArray();
        }
        public async Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var animeList = new List<AnimeModel>();
            var searchTasks = new List<Task<AnimeModel[]>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Commons.Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.SearchAnime(title, filter, cancellationToken));
                        break;
                    case Commons.Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.SearchAnime(title, filter, cancellationToken));
                        break;
                }
            }

            foreach (var task in searchTasks)
            {
                try
                {
                    animeList.AddRange(await task);
                }
                catch (Exception ex)
                {
                    // Logging
                }
            }

            return animeList.ToArray();
        }

        public async Task<EpisodeModel[]> GetLatestEpisode(int count, CancellationToken cancellationToken = default)
        {
            CheckForHost();

            var episodeList = new List<EpisodeModel>();
            var searchTasks = new List<Task<EpisodeModel[]>>();

            foreach (var host in CurrentHosts)
            {
                switch (host)
                {
                    case Commons.Hosts.AnimeSaturn:
                        searchTasks.Add(_animeSaturnManager.GetLatestEpisode(count, cancellationToken));
                        break;
                    case Commons.Hosts.AnimeUnity:
                        searchTasks.Add(_animeUnityManager.GetLatestEpisode(count, cancellationToken));
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
                    //Logging
                }
            }

            return episodeList.ToArray();
        }

        public async Task Download(EpisodeModel episode, CancellationToken cancellationToken = default)
        {
            Task task;
            switch (episode.Host)
            {
                case Hosts.AnimeSaturn:
                    task = _animeSaturnManager.Download(DownloadPath, episode, cancellationToken);
                    break;

                case Hosts.AnimeUnity:
                    task = _animeUnityManager.Download(DownloadPath, episode, cancellationToken);
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
                // Logging
            }
        }

        
        private void CheckForHost()
        {
            if(CurrentHosts.Length == 0) { throw new TenguException("No host defined"); }
        } 
    }
}
