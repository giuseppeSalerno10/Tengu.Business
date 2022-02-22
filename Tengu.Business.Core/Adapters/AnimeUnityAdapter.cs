using Downla;
using Flurl.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public class AnimeUnityAdapter : IAnimeUnityAdapter
    {

        private readonly IAnimeUnityUtilities _utilities;
        private readonly IDownlaClient _downlaCLient;


        public AnimeUnityAdapter(IAnimeUnityUtilities utilities, IDownlaClient downlaClient)
        {
            _utilities = utilities;
            _downlaCLient = downlaClient;
        }

        public async Task DownloadAsync(string downloadPath, string animeUrl, CancellationToken cancellationToken = default)
        {
            _downlaCLient.DownloadPath = downloadPath;
            _downlaCLient.DownloadAsync(new Uri(animeUrl), cancellationToken);
            await Task.Run(() => _downlaCLient.EnsureDownload(cancellationToken));
        }

        public async Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            var episodeList = new List<EpisodeModel>();

            var requestUrl = $"{Config.AnimeUnity.BaseAnimeUrl}/{animeId}";

            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(requestUrl, cancellationToken);

            var episodesJson = doc.GetElementbyId("anime")
                .SelectSingleNode("//video-player")
                .GetAttributeValue("episodes", "")
                .Replace("&quot;", "\"")
                .Replace("\\/", "/");

            var rawEpisodes = JsonConvert.DeserializeObject<AnimeUnityGetEpisodesOutput[]>(episodesJson);

            if (limit == 0) 
            { 
                limit = rawEpisodes.Length; 
            }

            for (int i = 0; i < limit; i++)
            {
                var episode = rawEpisodes[i];

                var episodeToAdd = new EpisodeModel()
                {
                    Title = $"Episodio {episode.Number}",
                    AnimeId = episode.AnimeId.ToString(),
                    Url = episode.Link,
                    EpisodeNumber = Convert.ToInt32(episode.Number),
                    Host = Hosts.AnimeUnity,
                    Id = episode.Link
                };

                episodeList.Add(episodeToAdd);
            }

            return episodeList.ToArray();
        }

        public async Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var episodeList = new List<EpisodeModel>();

            var requestUrl = $"{Config.AnimeUnity.BaseUrl}";

            requestUrl += $"/?page={1 + offset / 30}";

            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(requestUrl, cancellationToken);

            while(episodeList.Count < limit - offset)
            {
                var episodesJson = doc.DocumentNode
                    .SelectSingleNode("//layout-items")
                    .GetAttributeValue("items-json", "")
                    .Replace("&quot;", "\"")
                    .Replace("\\/", "/");

                var rawEpisodes = JsonConvert.DeserializeObject<AnimeUnityGetLatestEpisodesOutput>(episodesJson);

                if (limit == 0)
                {
                    limit = rawEpisodes.Data.Length;
                }
            
                for (int i = 0; i < limit; i++)
                {
                    var episode = rawEpisodes.Data[i];

                    var episodeToAdd = new EpisodeModel()
                    {
                        Title = $"AnimeID: {episode.AnimeId} - Episodio {episode.Number}",
                        AnimeId = episode.AnimeId.ToString(),
                        Url = episode.Link,
                        EpisodeNumber = Convert.ToInt32(episode.Number),
                        Host = Hosts.AnimeUnity,
                        Id = episode.Link
                    };

                    episodeList.Add(episodeToAdd);
                }

                requestUrl = rawEpisodes.Next_page_url;
            }

            return episodeList.ToArray();
        }

        public async Task<AnimeModel[]> SearchAsync(AnimeUnitySearchInput searchFilter, int count = 30, CancellationToken cancellationToken = default)
        {
            var animeList = new List<AnimeModel>();

            var requestUrl = Config.AnimeUnity.SearchUrl;

            var session = await _utilities.CreateSession();

            AnimeUnitySearchOutput response;

            do
            {
                response = await requestUrl
                .WithHeader("X-XSRF-TOKEN", session.XSRFToken)
                .WithCookie("XSRF-TOKEN", session.XSRFCookieToken)
                .WithCookie("animeunity_session", session.AnimeUnitySession)
                .PostJsonAsync(searchFilter)
                .ReceiveJson<AnimeUnitySearchOutput>();

                foreach (var record in response.Records)
                {
                    var anime = new AnimeModel()
                    {
                        Host = Hosts.AnimeUnity,
                        Id = $"{record.Id}-{record.Slug}",
                        Image = record.ImageUrl,
                        Title = record.Title,
                        Url = $"{Config.AnimeUnity.BaseAnimeUrl}/{record.Id}-{record.Slug}"
                    };
                    animeList.Add(anime);
                }

                searchFilter.Offset += 30;
            } while (animeList.Count < count && response.Records.Length > 0);

            return animeList.ToArray();
        }

    }
}
