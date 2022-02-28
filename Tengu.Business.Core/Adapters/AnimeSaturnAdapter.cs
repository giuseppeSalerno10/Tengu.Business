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

        public AnimeSaturnAdapter(IAnimeSaturnUtilities utilities)
        {
            _utilities = utilities;
        }

        public async Task<AnimeModel[]> SearchByTitleAsync(string title, int count = 30, CancellationToken cancellationToken = default)
        {
            var animeList = new List<AnimeModel>();

            var requestUrl = $"{Config.AnimeSaturn.SearchByTitleUrl}" +
                $"search={title}";

            var web = new HtmlWeb();

            HtmlDocument doc;
            doc = await web.LoadFromWebAsync($"{requestUrl}", cancellationToken);

            var animeNodes = doc.DocumentNode.SelectNodes($"//div[@class='container p-3 shadow rounded bg-dark-as-box']/ul[@class='list-group']");

            if (animeNodes != null)
            {
                foreach (var node in animeNodes)
                {
                    var urlNode = node.SelectSingleNode("./li/div[@class='item-archivio']/div[@class='info-archivio']/h3/a");

                    var url = urlNode.GetAttributeValue("href", "");
                    var anime = new AnimeModel()
                    {
                        Url = url,
                        Id = url.Split("/")[^1],
                        Host = Hosts.AnimeSaturn,
                        Title = urlNode.InnerText,
                        Image = node.SelectSingleNode("./li/div[@class='item-archivio']/a/img[@class='rounded locandina-archivio']")
                        .GetAttributeValue("src", "")
                    };
                    animeList.Add(anime);
                }
            }

            return animeList.ToArray();
        }

        public async Task<AnimeModel[]> SearchByFiltersAsync(AnimeSaturnSearchFilterInput searchFilter, int count = 30, CancellationToken cancellationToken = default)
        {
            var animeList = new ConcurrentBag<AnimeModel>();
            var web = new HtmlWeb();

            var requestUrl = $"{Config.AnimeSaturn.SearchByFilterUrl}";

            foreach (var state in searchFilter.Status)
            {
                requestUrl += $"states%5B0%5D={state}&";
            }

            foreach (var genre in searchFilter.Genres)
            {
                requestUrl += $"categories%5B0%5D={genre}&";
            }

            foreach (var year in searchFilter.Year)
            {
                requestUrl += $"years%5B0%5D={year}&";
            }

            HtmlDocument doc;
            doc = await web.LoadFromWebAsync($"{requestUrl}", cancellationToken);
            var totalPagesNode = doc.DocumentNode.SelectSingleNode("./div[@class='container p-3 shadow rounded bg-dark-as-box']/script");

            if (totalPagesNode != null)
            {
                var totalPages = Convert.ToInt32(totalPagesNode
                .InnerText
                .Split("totalPages: ")[1]
                .Split(",")[0]);

                var taskList = new List<Task>();

                var finalPage = totalPages > count / 15 ? count / 15 : totalPages;

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

        public async Task<EpisodeModel[]> GetEpisodesAsync(string animeId, int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var web = new HtmlWeb();
            HtmlDocument doc;

            var animeUrl = $"{Config.AnimeSaturn.BaseAnimeUrl}/{animeId}";

            doc = await web.LoadFromWebAsync($"{animeUrl}", cancellationToken);

            var episodesNodes = doc
                .GetElementbyId("resultsxd")
                .SelectNodes("./div/div/div");

            var episodeBag = new ConcurrentBag<EpisodeModel>();

            limit = limit == 0 ? episodesNodes.Count : limit;

            Parallel.For(offset, limit, index =>
            {
                if (index >= offset && index <= limit)
                {
                    var episodeUrl = episodesNodes[index]
                        .SelectSingleNode("./a")
                        .GetAttributeValue("href", "");

                    var url = GetAnimeStreamUrl(episodeUrl, cancellationToken).Result;

                    var episode = new EpisodeModel
                    {
                        Id = url.Split("=")[^1],
                        AnimeId = animeId,
                        Host = Hosts.AnimeSaturn,
                        Title = $"Episode {index}",
                        Url = url,
                        EpisodeNumber = index.ToString(),
                        DownloadUrl = url,
                    };
                    episodeBag.Add(episode);
                }
            });

            var episodesList = episodeBag
                .ToList();

            episodesList.Sort();

            return episodesList.ToArray();

        }

        public async Task<Calendar> GetCalendar(CancellationToken cancellationToken = default)
        {
            var days = (WeekDays[]) Enum.GetValues(typeof(WeekDays));

            var calendar = new Calendar() { Host = Hosts.AnimeSaturn };

            var url = Config.AnimeSaturn.CalendarUrl;
            var web = new HtmlWeb();

            var doc = await web.LoadFromWebAsync(url, cancellationToken);

            var entriesRows = doc.DocumentNode.SelectNodes("//tbody/tr");

            foreach(var row in entriesRows)
            {
                var entries = row.SelectNodes("./td");
                
                for (int i = 0; i < entries.Count; i++)
                {
                    if (!string.IsNullOrEmpty(entries[i].InnerText))
                    {
                        var entryToAdd = new CalendarEntryModel()
                        {
                            Anime = entries[i].InnerText,
                        };

                        var currentDay = days[i];

                        calendar.DaysDictionary[currentDay].Add(entryToAdd);
                    }
                }
            }

            return calendar;
        }

        public async Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var episodeList = new ConcurrentBag<EpisodeModel>();

            var doc = new HtmlDocument();

            var requestUrl = Config.AnimeSaturn.BaseLatestEpisodeUrl;

            var currentPage = offset / 15 + 1;

            var ignoreCount = offset - currentPage * 15;
            var episodeCount = limit - offset;

            while (!cancellationToken.IsCancellationRequested && episodeList.Count < episodeCount)
            {
                var response = await requestUrl
                    .WithHeader("X-Requested-With", "XMLHttpRequest")
                    .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                    .PostStringAsync($"page={currentPage}", cancellationToken)
                    .ReceiveString();

                doc.LoadHtml(response);

                var latestNodes = doc.DocumentNode.SelectNodes("./div/div[@class='anime-card main-anime-card']");

                await Parallel.ForEachAsync(latestNodes, async (node, cancellationToken) =>
                {
                    if (ignoreCount > 0)
                    {
                        ignoreCount--;
                    }
                    else if (episodeList.Count < episodeCount)
                    {
                        var urlNode = node.SelectSingleNode("./div[@class='card mb-4 shadow-sm']/a[1]");
                        var titleNode = node.SelectSingleNode("./div[@class='card mb-4 shadow-sm']/a[2]");

                        var url = urlNode.GetAttributeValue("href", "");
                        var streamUrl = await GetAnimeStreamUrl(url, cancellationToken);

                        var episode = new EpisodeModel()
                        {
                            Host = Hosts.AnimeSaturn,
                            Url = streamUrl,
                            Title = $"{urlNode.GetAttributeValue("title", "").Trim()} {titleNode.InnerText.Trim()}",
                            Image = urlNode.SelectSingleNode("./img").GetAttributeValue("src", ""),
                            Id = streamUrl.Split('=')[^1],
                            EpisodeNumber = (titleNode.InnerText.Trim().Split(" ")[^1]),
                            DownloadUrl = streamUrl
                        };

                        episodeList.Add(episode);
                    }
                });
                currentPage++;
            }

            return episodeList.ToArray();


        }

        public async Task<string> GetDownloadUrl(string episodeStreamUrl, CancellationToken cancellationToken = default)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(episodeStreamUrl, cancellationToken);

            var sourceNode = doc.DocumentNode.SelectSingleNode("//source");

            var serverNodes = doc.DocumentNode.SelectNodes("//a[@class='dropdown-item']");

            var url = "";

            if (sourceNode != null)
            {
                url = sourceNode.GetAttributeValue("src", "");
            }
            else
            {
                var nextPageUrl = serverNodes[0].GetAttributeValue("href", "");

                var internalWeb = new HtmlWeb();
                var internalDoc = await web.LoadFromWebAsync(nextPageUrl, cancellationToken);

                var siteRef = internalDoc.DocumentNode.SelectSingleNode("//div[@class='button']/a").GetAttributeValue("href", "");

                if (siteRef.Contains("streamtape"))
                {
                    siteRef = siteRef.Replace("/v/", "/e/");

                    internalDoc = await web.LoadFromWebAsync(siteRef, cancellationToken);

                    var cryptInfo = internalDoc.DocumentNode.SelectSingleNode("//script[6]").InnerText.Split("?id=")[1].Split("').")[0];

                    var informationUrl = $"https://streamtape.com/get_video?id={cryptInfo}&stream=1";

                    var infoResponse = await informationUrl
                        .WithAutoRedirect(false)
                        .HeadAsync(cancellationToken);

                    url = infoResponse.Headers.FirstOrDefault(header => header.Name == "Location").Value;

                }

            }

            return url;
        }

        private async Task<string> GetAnimeStreamUrl(string episodeUrl, CancellationToken cancellationToken)
        {
            var internalWeb = new HtmlWeb();
            var internalDoc = await internalWeb.LoadFromWebAsync(episodeUrl, cancellationToken);

            var url = internalDoc.DocumentNode
                    .SelectSingleNode("./div/div/div[@class='card-body']/a")
                    .GetAttributeValue("href", "");

            return url;
        }
       

    }
}
