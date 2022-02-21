using Downla;
using Flurl.Http;
using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public class AnimeSaturnAdapter : IAnimeSaturnAdapter
    {
        private readonly IAnimeSaturnUtilities _utilities;
        private readonly IDownlaClient _downlaClient;
        private readonly IM3u8Client _m3U8Client;

        public AnimeSaturnAdapter(IAnimeSaturnUtilities utilities, IDownlaClient downlaClient, IM3u8Client m3u8Client)
        {
            _utilities = utilities;
            _downlaClient = downlaClient;
            _m3U8Client = m3u8Client;
        }

        public async Task<AnimeModel[]> SearchByTitleAsync(string title, CancellationToken cancellationToken = default)
        {
            var requestUrl = $"{Config.AnimeSaturn.SearchByTitleUrl}" +
                $"search=1&" +
                $"key={title}";

            var response = requestUrl
                .GetJsonAsync<AnimeSaturnSearchTitleOutput[]>(cancellationToken);

            var animeList = new List<AnimeModel>();

            foreach (var item in await response)
            {
                var anime = new AnimeModel()
                {
                    Id = item.Link.Split("/")[^1],
                    Host = Hosts.AnimeSaturn,
                    Title = item.Name,
                    Image = item.Image,
                    Url = item.Link
                };

                animeList.Add(anime);
            }

            return animeList.ToArray();
        }

        public async Task<AnimeModel[]> SearchByFiltersAsync(AnimeSaturnSearchFilterInput searchFilter, CancellationToken cancellationToken = default)
        {
            var animeList = new ConcurrentBag<AnimeModel>();
            var web = new HtmlWeb();

            var requestUrl = $"{Config.AnimeSaturn.SearchByFilterUrl}";

            foreach (var state in searchFilter.Statuses)
            {
                requestUrl += $"states%5B0%5D={state}&";
            }

            foreach (var genre in searchFilter.Genres)
            {
                requestUrl += $"categories%5B0%5D={genre}&";
            }

            foreach (var year in searchFilter.Years)
            {
                requestUrl += $"years%5B0%5D={year}&";
            }

            requestUrl += $"language%5B0%5D={searchFilter.Language}&";

            HtmlDocument doc;
            doc = await web.LoadFromWebAsync($"{requestUrl}");
            var totalPagesNode = doc.DocumentNode.SelectSingleNode("./div[@class='container p-3 shadow rounded bg-dark-as-box']/script");

            if (totalPagesNode != null)
            {
                var totalPages = Convert.ToInt32(totalPagesNode
                .InnerText
                .Split("totalPages: ")[1]
                .Split(",")[0]);

                var taskList = new List<Task>();

                Parallel.For(1, totalPages + 1, i =>
                {
                    HtmlWeb innerWeb = new HtmlWeb();
                    HtmlDocument innerDoc = innerWeb.Load($"{requestUrl}page={i}");

                    var currentAnimesNode = innerDoc.DocumentNode.SelectNodes($"./div/div/div[@class='anime-card-newanime main-anime-card']");

                    foreach (var node in currentAnimesNode)
                    {
                        var aNode = node.SelectSingleNode("./div/a");

                        var url = aNode.GetAttributeValue("href", "");

                        var anime = new AnimeModel()
                        {
                            Id = url.Split("/")[^1],
                            Host = Hosts.AnimeSaturn,
                            Image = aNode.SelectSingleNode("./img").GetAttributeValue("src", ""),
                            Url = url,
                            Title = aNode.GetAttributeValue("title", ""),
                        };
                        animeList.Add(anime);
                    }
                });

            }

            return animeList.ToArray();
        }

        public async Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default)
        {
            var web = new HtmlWeb();
            HtmlDocument doc;

            var animeUrl = Config.AnimeSaturn.BaseAnimeUrl + animeId;

            doc = await web.LoadFromWebAsync($"{animeUrl}");

            var episodesNodes = doc
                .GetElementbyId("resultsxd")
                .SelectNodes("./div/div/div");

            var episodeList = new ConcurrentBag<EpisodeModel>();

            limit = limit == 0 ? episodesNodes.Count : limit;

            Parallel.For(0, episodesNodes.Count, index =>
            {
                if (index >= offset && index <= limit)
                {
                    var internalUrl = episodesNodes[index].SelectSingleNode("./a").GetAttributeValue("href", "");
                    var internalWeb = new HtmlWeb();
                    var internalDoc = internalWeb.Load(internalUrl);

                    var url = internalDoc.DocumentNode
                            .SelectSingleNode("./div/div/div[@class='card-body']/a")
                            .GetAttributeValue("href", "");

                    var title = internalDoc.DocumentNode
                            .SelectSingleNode("./div/div/div[@class='card-body']/h3[@class='text-center mb-4']")
                            .InnerText
                            .Split("<br>")[^1]
                            .Trim();

                    var episode = new EpisodeModel
                    {
                        Id = url.Split("/")[^1],
                        AnimeId = animeId,
                        Host = Hosts.AnimeSaturn,
                        Title = title,
                        Url = url,
                    };
                    episodeList.Add(episode);
                }

            });

            return episodeList.ToArray();

        }

        public async Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset, int limit, CancellationToken cancellationToken = default)
        {
            var episodeList = new List<EpisodeModel>();

            var doc = new HtmlDocument();

            var requestUrl = Config.AnimeSaturn.BaseLatestEpisodeUrl;

            var currentPage = offset / 15;

            var ignoreCount = offset - currentPage * 15;
            var episodeCount = offset - limit;

            while (!cancellationToken.IsCancellationRequested && episodeList.Count < episodeCount)
            {
                var response = await requestUrl
                    .WithHeader("X-Requested-With", "XMLHttpRequest")
                    .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                    .PostStringAsync($"page={currentPage}", cancellationToken)
                    .ReceiveString();

                doc.LoadHtml(response);

                var latestNodes = doc.DocumentNode.SelectNodes("./div/div[@class='anime-card main-anime-card']");

                foreach (var node in latestNodes)
                {
                    if(ignoreCount > 0)
                    {
                        ignoreCount--;
                    }
                    else if (episodeList.Count < episodeCount)
                    {
                        var urlNode = node.SelectSingleNode("./div[@class='card mb-4 shadow-sm']/a[1]");
                        var titleNode = node.SelectSingleNode("./div[@class='card mb-4 shadow-sm']/a[2]");

                        var episode = new EpisodeModel()
                        {
                            Host = Hosts.AnimeSaturn,
                            Url = urlNode.GetAttributeValue("href", ""),
                            Title = $"{urlNode.GetAttributeValue("title", "").Trim()} {titleNode.InnerText.Trim()}",
                            Image = urlNode.SelectSingleNode("./img").GetAttributeValue("src", "")
                        };
                        episodeList.Add(episode);
                    }
                }

                currentPage++;
            }

            return episodeList.ToArray();


        }

        public async Task DownloadAsync(string downloadPath, string episodeId, CancellationToken cancellationToken = default)
        {
            var web = new HtmlWeb();

            var episodeUrl = Config.AnimeSaturn.BaseUrl + episodeId;

            HtmlDocument doc = await web.LoadFromWebAsync(episodeUrl, cancellationToken);

            if (doc.DocumentNode.SelectSingleNode("//center/div/div/div/div/div/div/div/script[2]") != null) //M3U8
            {
                var scriptNode = doc.DocumentNode.SelectSingleNode("//center/div/div/div/div/div/div/div/script[2]").InnerText;

                var filename = doc.DocumentNode.SelectSingleNode("//center/div/div/div/h4[@class='text-white mb-3']").InnerText;

                var m3u8InitialUrl = scriptNode.Split("file: ")[1]
                    .Split(",")[0]
                    .Replace("\"", "");

                _m3U8Client.DownloadPath = downloadPath;

                var downloadUrls = await _m3U8Client.GenerateDownloadUrls(m3u8InitialUrl, cancellationToken);

                await _m3U8Client.Download($"{filename}.mp4", downloadUrls, cancellationToken);
            } //TS
            else if (doc.GetElementbyId("myvideo") != null)
            {
                var downloadUrl = doc.GetElementbyId("myvideo")
                    .SelectSingleNode("./source")
                    .GetAttributeValue("src", "");

                _downlaClient.DownloadPath = downloadPath;

                _downlaClient.DownloadAsync(new Uri(downloadUrl), cancellationToken);
                _downlaClient.EnsureDownload(cancellationToken);
            } //Direct
            else if (doc.DocumentNode.SelectSingleNode("./div[@class=button]/a") != null)
            {
                var streamTapeUrl = doc.DocumentNode
                    .SelectSingleNode("./div[@class=button]/a")
                    .GetAttributeValue("href", "")
                    .Replace("https://streamtape.com/v", "https://streamtape.com/e");

                doc = web.Load(streamTapeUrl);

                var downloadUrl = doc.GetElementbyId("robotlink")
                    .InnerText
                    .Replace("\"", "");

                _downlaClient.DownloadPath = downloadPath;

                _downlaClient.DownloadAsync(new Uri(downloadUrl), cancellationToken);
                _downlaClient.EnsureDownload(cancellationToken);
            } //StreamTape
            else
            {
                throw new TenguException("No download method found");
            }
        }
    }
}
