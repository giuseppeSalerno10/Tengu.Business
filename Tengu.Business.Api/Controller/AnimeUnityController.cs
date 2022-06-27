﻿using Downla;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.API.DTO;
using Tengu.Business.API.Managers.Interfaces;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Commons.Services.Interfaces;

namespace Tengu.Business.API.Controller
{
    public class AnimeUnityController : IAnimeUnityController
    {
        private readonly IAnimeUnityManager _manager;
        private readonly IManipulationService _manipulationService;

        public AnimeUnityController(IAnimeUnityManager animeUnityManager, IManipulationService manipulationService)
        {
            _manager = animeUnityManager;
            _manipulationService = manipulationService;
        }

        public async Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AnimeModel[]>() { Host = Hosts.AnimeUnity };

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

        public async Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(SearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AnimeModel[]>() { Host = Hosts.AnimeUnity };

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

        public async Task<OperationResult<AnimeModel[]>> SearchAnimeAsync(string title, SearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var result = new OperationResult<AnimeModel[]>() { Host = Hosts.AnimeUnity };

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

        public async Task<OperationResult<EpisodeModel[]>> GetEpisodesAsync(string animeId, int offset, int limit, CancellationToken cancellationToken)
        {
            var result = new OperationResult<EpisodeModel[]>() { Host = Hosts.AnimeUnity };

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

        public async Task<OperationResult<EpisodeModel[]>> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken)
        {
            var result = new OperationResult<EpisodeModel[]>() { Host = Hosts.AnimeUnity };

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
            var result = new OperationResult<Calendar>() { Host = Hosts.AnimeUnity };

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

        public OperationResult<DownloadInfosModel> DownloadAsync(string downloadPath, string episodeUrl, CancellationToken cancellationToken)
        {
            var result = new OperationResult<DownloadInfosModel>() { Host = Hosts.AnimeUnity };

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
