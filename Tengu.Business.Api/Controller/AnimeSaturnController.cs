using Downla;
using Downla.Models;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.API.DTO;
using Tengu.Business.API.Managers.Interfaces;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Commons.Services.Interfaces;

namespace Tengu.Business.API.Controller
{
    public class AnimeSaturnController : IAnimeSaturnController
    {
        private readonly IAnimeSaturnManager _manager;
        private readonly IManipulationService _manipulationService;

        public AnimeSaturnController(IAnimeSaturnManager manager, IManipulationService manipulationService)
        {
            _manager = manager;
            _manipulationService = manipulationService;
        }

        public void UpdateDownlaSettings(string? downloadPath, int maxConnections, long maxPacketSize)
        {
            _manager.UpdateDownlaSettings(downloadPath, maxConnections, maxPacketSize);
        }

        public async Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AnimeModel[]>() { Host = TenguHosts.AnimeSaturn };

            try
            {
                result.Data = await _manager.SearchAnimeAsync(title, count, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }

        public async Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(TenguSearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AnimeModel[]>() { Host = TenguHosts.AnimeSaturn };

            try
            {
                result.Data = await _manager.SearchAnimeAsync(filter, count, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }

        public async Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(string title, TenguSearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AnimeModel[]>() { Host = TenguHosts.AnimeSaturn };

            try
            {
                result.Data = await _manager.SearchAnimeAsync(title, filter, count, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }

        public async Task<OperationResult<EpisodeModel[]>> GetEpisodesAsync(string animeId, int offset, int count, CancellationToken cancellationToken)
        {
            var result = new OperationResult<EpisodeModel[]>() { Host = TenguHosts.AnimeSaturn };

            try
            {
                result.Data = await _manager.GetEpisodesAsync(animeId, offset, count, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }

        public async Task<OperationResult<EpisodeModel[]>> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken)
        {
            var result = new OperationResult<EpisodeModel[]>() { Host = TenguHosts.AnimeSaturn };

            try
            {
                result.Data = await _manager.GetLatestEpisodesAsync(offset, limit, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }

        public async Task<OperationResult<Calendar>> GetCalendar(CancellationToken cancellationToken)
        {
            var result = new OperationResult<Calendar>() { Host = TenguHosts.AnimeSaturn };

            try
            {
                result.Data = await _manager.GetCalendar(cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }

        public OperationResult<DownloadMonitor> DownloadAsync(string episodeUrl, out Task downloadTask, CancellationToken cancellationToken)
        {
            var result = new OperationResult<DownloadMonitor>() { Host = TenguHosts.AnimeSaturn };

            try
            {
                result.Data = _manager.DownloadAsync(episodeUrl, out downloadTask, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
                downloadTask = Task.FromException(e);
            }

            return result;
        }
    }
}
