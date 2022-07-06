using Flurl.Http;
using HtmlAgilityPack;
using System.Collections.Concurrent;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.Adapters.Interfaces;
using Tengu.Business.Core.DTO.Input.AnimeSaturn;
using Tengu.Business.Core.Utilities.Interfaces;

namespace Tengu.Business.Core.Adapters
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

            var requestUrl = $"{Config.AnimeSaturnConfig.SearchByTitleUrl}" +
                $"search={title}";

            var web = new HtmlWeb();

            HtmlDocument doc;
            doc = await web.LoadFromWebAsync($"{requestUrl}", cancellationToken);

            var animeNodes = doc.DocumentNode.SelectNodes($"//div[@class='container p-3 shadow rounded bg-dark-as-box']/ul[@class='list-group']");

            if (animeNodes != null)
            {
                foreach (var node in animeNodes)
                {
                    if (animeList.Count < count)
                    {
                        var urlNode = node.SelectSingleNode("./li/div[@class='item-archivio']/div[@class='info-archivio']/h3/a");

                        var url = urlNode.GetAttributeValue("href", "");
                        var anime = new AnimeModel()
                        {
                            Url = url,
                            Id = url.Split("/")[^1],
                            Host = TenguHosts.AnimeSaturn,
                            Title = urlNode.InnerText,
                            Image = node.SelectSingleNode("./li/div[@class='item-archivio']/a/img[@class='rounded locandina-archivio']")
                            .GetAttributeValue("src", "")
                        };
                        animeList.Add(anime);
                    }
                }
            }

            return animeList.ToArray();
        }

        public async Task<AnimeModel[]> SearchByFiltersAsync(AnimeSaturnSearchFilterInput searchFilter, int count = 30, CancellationToken cancellationToken = default)
        {
            var animeList = new ConcurrentBag<AnimeModel>();
            var web = new HtmlWeb();

            var requestUrl = $"{Config.AnimeSaturnConfig.SearchByFilterUrl}";

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
                            Host = TenguHosts.AnimeSaturn,
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

            var animeUrl = $"{Config.AnimeSaturnConfig.BaseAnimeUrl}/{animeId}";

            doc = await web.LoadFromWebAsync($"{animeUrl}", cancellationToken);

            var episodeBag = new ConcurrentBag<EpisodeModel>();

            var animeTitle = doc.DocumentNode.SelectSingleNode("//div[@class='container anime-title-as mb-3 w-100']/b").InnerText;
            var episodes = doc.DocumentNode.SelectNodes("//div[@class='btn-group episodes-button episodi-link-button']/a");

            limit = limit == 0 ? episodes.Count : limit;

            Parallel.For(offset, limit, index =>
            {
                if (index >= offset && index <= limit)
                {
                    var episodeUrl = episodes[index]
                        .GetAttributeValue("href", "");

                    var url = GetEpisodeUrl(episodeUrl);

                    var episode = new EpisodeModel
                    {
                        Id = url.Split("=")[^1],
                        AnimeId = animeId,
                        Host = TenguHosts.AnimeSaturn,
                        Title = animeTitle,
                        Url = url,
                        EpisodeNumber = (index + 1).ToString(),
                        DownloadUrl = url,
                    };
                    episodeBag.Add(episode);
                }
                Task.Delay(500).Wait();
            });

            var episodesList = episodeBag
                .ToList();

            episodesList.Sort();

            return episodesList.ToArray();

        }

        public async Task<Calendar> GetCalendar(CancellationToken cancellationToken = default)
        {
            var days = (TenguWeekDays[])Enum.GetValues(typeof(TenguWeekDays));

            var calendar = new Calendar() { Host = TenguHosts.AnimeSaturn };

            var url = Config.AnimeSaturnConfig.CalendarUrl;
            var web = new HtmlWeb();

            var doc = await web.LoadFromWebAsync(url, cancellationToken);

            var entriesRows = doc.DocumentNode.SelectNodes("//tbody/tr");

            foreach (var row in entriesRows)
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

        public async Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 15, CancellationToken cancellationToken = default)
        {
            var episodeList = new List<EpisodeModel>();

            var doc = new HtmlDocument();

            var requestUrl = Config.AnimeSaturnConfig.BaseLatestEpisodeUrl;

            var currentPage = offset / 15;

            var count = limit - offset;

            var session = await _utilities.CreateSession();

            while (!cancellationToken.IsCancellationRequested && count > 0)
            {
                var startIndex = offset - currentPage * 15;
                var endIndex = startIndex + count;

                var response = await requestUrl
                    .WithHeader("X-Requested-With", "XMLHttpRequest")
                    .WithCookie("PHPSESSID", session.SessionId)
                    .SendUrlEncodedAsync(HttpMethod.Get, $"page={currentPage + 1}", cancellationToken)
                    .ReceiveString();

                doc.LoadHtml(response);

                var latestNodes = doc.DocumentNode.SelectNodes("./div/div[@class='anime-card main-anime-card']");

                for (int i = startIndex; i < endIndex; i++)
                {
                    var node = latestNodes[i];

                    var urlNode = node.SelectSingleNode("./div[@class='card mb-4 shadow-sm']/a[1]");
                    var titleNode = node.SelectSingleNode("./div[@class='card mb-4 shadow-sm']/a[2]");

                    var url = urlNode.GetAttributeValue("href", "");
                    var streamUrl = GetEpisodeUrl(url);

                    var episode = new EpisodeModel()
                    {
                        Host = TenguHosts.AnimeSaturn,
                        Url = streamUrl,
                        Title = $"{urlNode.GetAttributeValue("title", "").Trim()}",
                        Image = urlNode.SelectSingleNode("./img").GetAttributeValue("src", ""),
                        Id = streamUrl.Split('=')[^1],
                        EpisodeNumber = titleNode.InnerText.Trim().Split(" ")[^1],
                        DownloadUrl = streamUrl
                    };

                    episodeList.Add(episode);
                }

                count -= endIndex - startIndex;
                offset = ++currentPage * 15;
            }

            return episodeList.ToArray();


        }

        public async Task<string> GetDownloadUrl(string episodeUrl, CancellationToken cancellationToken = default)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(episodeUrl, cancellationToken);

            var sourceNode = doc.DocumentNode.SelectSingleNode("//source");

            string downloadUrl;

            if (sourceNode != null)
            {
                downloadUrl = sourceNode.GetAttributeValue("src", "");
            }
            else
            {
                downloadUrl = await GetStreamUrl(episodeUrl, cancellationToken);
            }

            return downloadUrl;
        }
        public async Task<string> GetStreamUrl(string episodeUrl, CancellationToken cancellationToken = default)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(episodeUrl, cancellationToken);

            var streamNode = doc.DocumentNode.SelectNodes("./div[@class='embed-responsive-item']/script[@type='text/javascript']")[1];

            var rawUrl = streamNode.InnerText.Split("file:")[1];
            rawUrl = rawUrl.Split("tracks")[0];

            var streamUrl = rawUrl.Trim().Replace("\"", "");

            return streamUrl;
        }


        private string GetEpisodeUrl(string episodeUrl)
        {
            var internalWeb = new HtmlWeb();
            var internalDoc = internalWeb.Load(episodeUrl);

            var url = internalDoc.DocumentNode
                    .SelectSingleNode("./div/div/div[@class='card-body']/a")
                    .GetAttributeValue("href", "");

            return url;
        }
    }
}
