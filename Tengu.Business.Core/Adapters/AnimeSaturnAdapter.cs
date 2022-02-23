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
            doc = await web.LoadFromWebAsync($"{requestUrl}");

            var animeNodes = doc.DocumentNode.SelectNodes($"//div[@class='container p-3 shadow rounded bg-dark-as-box']/ul[@class='list-group']");

            if(animeNodes != null)
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
            doc = await web.LoadFromWebAsync($"{requestUrl}");
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

            doc = await web.LoadFromWebAsync($"{animeUrl}");

            var episodesNodes = doc
                .GetElementbyId("resultsxd")
                .SelectNodes("./div/div/div");

            var episodeBag = new ConcurrentBag<EpisodeModel>();

            limit = limit == 0 ? episodesNodes.Count : limit;

            for (int index = offset; index < limit; index++)
            {
                if (index >= offset && index <= limit)
                {
                    var episodeUrl = episodesNodes[index]
                        .SelectSingleNode("./a")
                        .GetAttributeValue("href", "");
                    var url = await GetAnimeStreamUrl(episodeUrl);
                    var downloadUrl = await GetDownloadUrl(url);

                    var episode = new EpisodeModel
                    {
                        Id = url.Split("=")[^1],
                        AnimeId = animeId,
                        Host = Hosts.AnimeSaturn,
                        Title = $"Episode {index}",
                        Url = animeUrl,
                        EpisodeNumber = index
                    };
                    episodeBag.Add(episode);
                }

            }

            var episodesList = episodeBag
                .ToList();

            episodesList.Sort();

            return episodesList.ToArray();

        }

        public async Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var episodeList = new List<EpisodeModel>();

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

                        var url = urlNode.GetAttributeValue("href", "");
                        var streamUrl = await GetAnimeStreamUrl(url);
                        var downloadUrl = await GetDownloadUrl(streamUrl);

                        var episode = new EpisodeModel()
                        {
                            Host = Hosts.AnimeSaturn,
                            Url = streamUrl,
                            Title = $"{urlNode.GetAttributeValue("title", "").Trim()} {titleNode.InnerText.Trim()}",
                            Image = urlNode.SelectSingleNode("./img").GetAttributeValue("src", ""),
                            Id = streamUrl.Split('=')[^1],
                            DownloadUrl = downloadUrl,
                        };

                        episodeList.Add(episode);
                    }
                }

                currentPage++;
            }

            return episodeList.ToArray();


        }
        

        private async Task<string> GetAnimeStreamUrl(string episodeUrl)
        {
            var internalWeb = new HtmlWeb();
            var internalDoc = await internalWeb.LoadFromWebAsync(episodeUrl);

            var url = internalDoc.DocumentNode
                    .SelectSingleNode("./div/div/div[@class='card-body']/a")
                    .GetAttributeValue("href", "");

            return url;
        }
        private async Task<string> GetDownloadUrl(string episodeStreamUrl)
        {
            var internalWeb = new HtmlWeb();
            var internalDoc = await internalWeb.LoadFromWebAsync(episodeStreamUrl);

            var url = internalDoc.DocumentNode
                    .SelectSingleNode("//source")
                    .GetAttributeValue("src", "");

            return url;
        }

    }
}
