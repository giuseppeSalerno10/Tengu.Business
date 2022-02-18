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

        public async Task<AnimeModel[]> SearchByTitle(string title, CancellationToken cancellationToken = default)
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
                    Host = Hosts.AnimeSaturn,
                    Title = item.Name,
                    Image = item.Image,
                    Url = item.Link
                };

                animeList.Add(anime);
            }

            return await _utilities.FillAnimeList(animeList, cancellationToken);
        }

        public async Task<AnimeModel[]> SearchByFilters(AnimeSaturnSearchFilterInput searchFilter, CancellationToken cancellationToken = default)
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

                for (int i = 1; i <= totalPages; i++)
                {
                    taskList.Add(Task.Run(async () =>
                    {
                        HtmlWeb innerWeb = new HtmlWeb();
                        HtmlDocument innerDoc = await innerWeb.LoadFromWebAsync($"{requestUrl}page={i}");

                        var currentAnimesNode = innerDoc.DocumentNode.SelectNodes($"./div/div/div[@class='anime-card-newanime main-anime-card']");

                        foreach (var node in currentAnimesNode)
                        {
                            var aNode = node.SelectSingleNode("./div/a");

                            var anime = new AnimeModel()
                            {
                                Host = Hosts.AnimeSaturn,
                                Image = aNode.SelectSingleNode("./img").GetAttributeValue("src", ""),
                                Url = aNode.GetAttributeValue("href", ""),
                                Title = aNode.GetAttributeValue("title", ""),
                            };
                            animeList.Add(anime);
                        }
                    }));
                }

                foreach (var task in taskList)
                {
                    await task;
                }

            }
            return await _utilities.FillAnimeList(animeList.ToArray());
        }

        public async Task<EpisodeModel[]> GetLatestEpisode(int count, CancellationToken cancellationToken = default)
        {
            var episodeList = new List<EpisodeModel>();

            var doc = new HtmlDocument();

            var requestUrl = Config.AnimeSaturn.BaseLatestEpisodeUrl;

            var currentAnimes = 0;
            var currentPage = 1;

            while (!cancellationToken.IsCancellationRequested && currentAnimes < count)
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

                    currentAnimes++;

                }

                currentPage++;
            }

            return episodeList.ToArray();


        }

        public async Task Download(string downloadPath, string animeUrl, string fileName, CancellationToken cancellationToken = default)
        {
            var web = new HtmlWeb();
            HtmlDocument doc = await web.LoadFromWebAsync(animeUrl, cancellationToken);

            if (doc.DocumentNode.SelectSingleNode("//center/div/div/div/div/div/div/div/script[2]") != null) //M3U8
            {
                var scriptNode = doc.DocumentNode.SelectSingleNode("//center/div/div/div/div/div/div/div/script[2]").InnerText;

                var m3u8InitialUrl = scriptNode.Split("file: ")[1]
                    .Split(",")[0]
                    .Replace("\"", "");

                _m3U8Client.DownloadPath = downloadPath;

                var downloadUrls = await _m3U8Client.GenerateDownloadUrls(m3u8InitialUrl, cancellationToken);

                await _m3U8Client.Download($"{fileName}.mp4", downloadUrls, cancellationToken);
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
