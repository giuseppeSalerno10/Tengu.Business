using Downla;
using Tengu.Business.API.DTO;
using Tengu.Business.API.Managers.Interfaces;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.Adapters.Interfaces;
using Tengu.Business.Core.DTO.Input.AnimeUnity;
using Tengu.Business.Core.Utilities.Interfaces;

namespace Tengu.Business.API.Managers
{
    public class AnimeUnityManager : IAnimeUnityManager
    {
        private readonly IAnimeUnityAdapter _adapter;
        private readonly IAnimeUnityUtilities _utilities;
        private readonly IDownlaClient _downlaClient;

        public AnimeUnityManager(IAnimeUnityAdapter adapter, IAnimeUnityUtilities utilities, IDownlaClient client)
        {
            _adapter = adapter;
            _utilities = utilities;
            _downlaClient = client;
        }

        public DownloadInfosModel DownloadAsync(string downloadPath, string episodeUrl, CancellationToken cancellationToken = default)
        {
            _downlaClient.DownloadPath = downloadPath;

            _downlaClient.MaxPacketSize = Config.Common.PacketSize;
            _downlaClient.MaxConnections = Config.Common.Connections;

            return _downlaClient.StartDownload(new Uri(episodeUrl), cancellationToken);
        }

        public Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            return _adapter.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
        }

        public Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            return _adapter.GetLatestEpisodesAsync(offset, limit, cancellationToken);
        }
        public Task<Calendar> GetCalendar(CancellationToken cancellationToken = default)
        {
            return _adapter.GetCalendar(cancellationToken);
        }
        public Task<AnimeModel[]> SearchAnimeAsync(SearchFilter filter, int count = 30, CancellationToken cancellationToken = default)
        {
            var adapterFilter = new AnimeUnitySearchInput();

            if (filter.Genres.Count() > 0)
            {
                adapterFilter.Genres = _utilities.GetGenreArray(filter.Genres);
            }

            if (filter.Status != Statuses.None)
            {
                adapterFilter.Status = _utilities.GetStatus(filter.Status);
            }

            return _adapter.SearchAsync(adapterFilter, count, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnimeAsync(string title, int count = 30, CancellationToken cancellationToken = default)
        {
            var adapterFilter = new AnimeUnitySearchInput()
            {
                Title = title,
            };

            return _adapter.SearchAsync(adapterFilter, count, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnimeAsync(string title, SearchFilter filter, int count = 30, CancellationToken cancellationToken = default)
        {
            var adapterFilter = new AnimeUnitySearchInput()
            {
                Title = title,
            };

            if (filter.Genres.Count() > 0)
            {
                adapterFilter.Genres = _utilities.GetGenreArray(filter.Genres);
            }

            if (filter.Status != Statuses.None)
            {
                adapterFilter.Status = _utilities.GetStatus(filter.Status);
            }

            return _adapter.SearchAsync(adapterFilter, count, cancellationToken);
        }
    }
}
