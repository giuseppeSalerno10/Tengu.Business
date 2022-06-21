using Downla;
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

        public async Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken)
        {
            var result = new TenguResult<AnimeModel[]>() { Host = Hosts.AnimeSaturn };

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

        public async Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(SearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var result = new TenguResult<AnimeModel[]>() { Host = Hosts.AnimeSaturn };

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

        public async Task<TenguResult<AnimeModel[]>> SearchAnimeAsync(string title, SearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var result = new TenguResult<AnimeModel[]>() { Host = Hosts.AnimeSaturn };

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

        public async Task<TenguResult<EpisodeModel[]>> GetEpisodesAsync(string animeId, int offset, int limit, CancellationToken cancellationToken)
        {
            var result = new TenguResult<EpisodeModel[]>() { Host = Hosts.AnimeSaturn };

            try
            {
                result.Data = await _manager.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }

        public async Task<TenguResult<EpisodeModel[]>> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken)
        {
            var result = new TenguResult<EpisodeModel[]>() { Host = Hosts.AnimeSaturn };

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

        public async Task<TenguResult<Calendar>> GetCalendar(CancellationToken cancellationToken)
        {
            var result = new TenguResult<Calendar>() { Host = Hosts.AnimeSaturn };

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

        public TenguResult<DownloadInfosModel> DownloadAsync(string downloadPath, string episodeUrl, CancellationToken cancellationToken)
        {
            var result = new TenguResult<DownloadInfosModel>() { Host = Hosts.AnimeSaturn };

            try
            {
                result.Data = _manager.DownloadAsync(downloadPath, episodeUrl, cancellationToken);
                result.Success = true;
            }
            catch (Exception e)
            {
                _manipulationService.HandleTenguException(e, ref result);
            }

            return result;
        }
    }
}
