using Downla.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using TenguUI.Controllers.Interfaces;
using TenguUI.Managers.Interfaces;

namespace TenguUI.Controllers
{
    public class TenguController : ITenguController
    {
        private readonly ITenguManager _manager;
        private readonly ILogger<TenguController> _logger;

        public TenguController(ITenguManager manager, ILogger<TenguController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public void SetHosts(TenguHosts[] hosts)
        {
            _manager.SetHosts(hosts);
        }

        public async Task<AnimeModel[]> SearchAnimesAsync(string title, TenguSearchFilter searchFilter)
        {
            var result = Array.Empty<AnimeModel>();

            try
            {
                //result = await _manager.SearchAnimes(title);
                //result = await _manager.SearchAnimes(searchFilter);
                result = await _manager.SearchAnimesAsync(title, searchFilter);
            }
            catch (Exception e)
            {
                _logger.LogError($"SearchAnimes - Message {e.Message}");
            }

            return result;
        }

        public async Task<EpisodeModel[]> GetEpisodesAsync(string animeId, TenguHosts animeHost, int offset = 0, int limit = 0)
        {
            var result = Array.Empty<EpisodeModel>();

            try
            {
                result = await _manager.GetEpisodesAsync(animeId, animeHost, offset, limit);
            }
            catch (Exception e)
            {
                _logger.LogError($"SearchAnimes - Message {e.Message}");
            }

            return result;
        }

        public DownloadMonitor DownloadAsync(string episodeUrl, TenguHosts episodeHost)
        {
            DownloadMonitor result = new DownloadMonitor();

            try
            {
                result = _manager.DownloadAsync(episodeUrl, episodeHost);
            }
            catch (Exception e)
            {
                _logger.LogError($"SearchAnimes - Message {e.Message}");
            }

            return result;
        }
    }
}
