using Downla;
using Downla.Interfaces;
using Downla.Models;
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

        public void UpdateDownlaSettings(string? downloadPath, int maxConnections, long maxPacketSize)
        {
            _downlaClient.DownloadPath = downloadPath != null ? downloadPath : _downlaClient.DownloadPath;
            _downlaClient.MaxConnections = maxConnections != default ? maxConnections : _downlaClient.MaxConnections;
            _downlaClient.MaxPacketSize = maxPacketSize != default ? maxPacketSize : _downlaClient.MaxPacketSize;
        }

        public DownloadMonitor DownloadAsync(string episodeUrl, out Task downloadTask, CancellationToken cancellationToken)
        {
            downloadTask = _downlaClient.StartFileDownloadAsync(new Uri(episodeUrl), out DownloadMonitor downloadMonitor, ct: cancellationToken);
            return downloadMonitor;
        }

        public Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset, int limit, CancellationToken cancellationToken)
        {
            return _adapter.GetEpisodesAsync(animeId, offset, limit, cancellationToken);
        }

        public Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken)
        {
            return _adapter.GetLatestEpisodesAsync(offset, limit, cancellationToken);
        }
        public Task<Calendar> GetCalendar(CancellationToken cancellationToken)
        {
            return _adapter.GetCalendar(cancellationToken);
        }
        public Task<AnimeModel[]> SearchAnimeAsync(TenguSearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var adapterFilter = new AnimeUnitySearchInput();

            if (filter.Genres.Count() > 0)
            {
                adapterFilter.Genres = _utilities.GetGenreArray(filter.Genres);
            }

            if (filter.Status != TenguStatuses.None)
            {
                adapterFilter.Status = _utilities.GetStatus(filter.Status);
            }

            return _adapter.SearchAsync(adapterFilter, count, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnimeAsync(string title, int count, CancellationToken cancellationToken)
        {
            var adapterFilter = new AnimeUnitySearchInput()
            {
                Title = title,
            };

            return _adapter.SearchAsync(adapterFilter, count, cancellationToken);
        }

        public Task<AnimeModel[]> SearchAnimeAsync(string title, TenguSearchFilter filter, int count, CancellationToken cancellationToken)
        {
            var adapterFilter = new AnimeUnitySearchInput()
            {
                Title = title,
            };

            if (filter.Genres.Count() > 0)
            {
                adapterFilter.Genres = _utilities.GetGenreArray(filter.Genres);
            }

            if (filter.Status != TenguStatuses.None)
            {
                adapterFilter.Status = _utilities.GetStatus(filter.Status);
            }

            return _adapter.SearchAsync(adapterFilter, count, cancellationToken);
        }
    }
}
