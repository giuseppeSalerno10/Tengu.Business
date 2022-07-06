using Downla.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API.DTO;
using Tengu.Business.API.Interfaces;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using TenguUI.Managers.Interfaces;

namespace TenguUI.Managers
{
    public class TenguManager : ITenguManager
    {
        private readonly ITenguApi _tenguApi;
        private readonly ILogger<TenguManager> _logger;

        public TenguManager(ITenguApi tenguApi, ILogger<TenguManager> logger)
        {
            _tenguApi = tenguApi;
            _logger = logger;
        }

        public void SetHosts(TenguHosts[] hosts)
        {
            _tenguApi.CurrentHosts = hosts;
        }

        public async Task<AnimeModel[]> SearchAnimesAsync(string title)
        {
            TenguResult<AnimeModel[]> searchResult;

            searchResult = await _tenguApi.SearchAnimeAsync(title);

            CheckForError(searchResult);

            return searchResult.Data;
        }
        public async Task<AnimeModel[]> SearchAnimesAsync(TenguSearchFilter filter)
        {
            TenguResult<AnimeModel[]> searchResult;

            searchResult = await _tenguApi.SearchAnimeAsync(filter);
            CheckForError(searchResult);

            return searchResult.Data;
        }
        public async Task<AnimeModel[]> SearchAnimesAsync(string title, TenguSearchFilter filter)
        {
            TenguResult<AnimeModel[]> searchResult;

            searchResult = await _tenguApi.SearchAnimeAsync(title, filter);
            CheckForError(searchResult);

            return searchResult.Data;
        }

        public async Task<EpisodeModel[]> GetEpisodesAsync(string animeId, TenguHosts animeHost, int offset, int limit)
        {
            TenguResult<EpisodeModel[]> searchResult;

            searchResult = await _tenguApi.GetEpisodesAsync(animeId, animeHost, offset, limit);
            CheckForError(searchResult);

            return searchResult.Data;

        }

        public DownloadMonitor DownloadAsync(string episodeUrl, TenguHosts episodeHost)
        {
            TenguResult<DownloadMonitor> searchResult;
            searchResult = _tenguApi.DownloadAsync(episodeUrl, episodeHost, out _);
            CheckForError(searchResult);

            return searchResult.Data;

        }


        private void CheckForError(TenguResult result)
        {
            var errors = result.Infos.Where( info => info.Success == false);
            foreach(var error in errors)
            {
                _logger.LogError($"Error! {error.Exception.Message}");
            }
        }
    }
}
