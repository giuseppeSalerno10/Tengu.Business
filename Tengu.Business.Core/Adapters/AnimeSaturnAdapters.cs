using Downla;
using Flurl.Http;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public class AnimeSaturnAdapters
    {
        public AnimeModel[] SearchByTitle(string title) 
        {
            var requestUrl = $"{Config.AnimeSaturn.SearchByTitleUrl}" +
                $"search=1&" +
                $"key={title}";

            var response = requestUrl
                .GetJsonAsync<AnimeSaturnSearchTitleOutput[]>()
                .Result;

            var animeList = new List<AnimeModel>();

            foreach (var item in response)
            {
                var anime = new AnimeModel()
                {
                    Title = item.Name,
                    Image = item.Image,
                    Url = item.Link
                };

                animeList.Add(anime);
            }

            AnimeSaturnUtilities.FillAnimeList(animeList);

            return animeList.ToArray();
        }

        public AnimeModel[] SearchByFilters(AnimeSaturnSearchFilterInput searchFilter)
        {
            var web = new HtmlWeb();

            var animeList = new List<AnimeModel>();

            //Prima query Get Page
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
            doc = web.Load($"{requestUrl}");
            
            int currentPage = 1;
            var paginationNode = doc.GetElementbyId("pagination");

            while (paginationNode != null)
            {
                var currentAnimesNode = doc.DocumentNode.SelectNodes($"./div/div/div[@class='anime-card-newanime main-anime-card']");

                foreach (var node in currentAnimesNode)
                {
                    var aNode = node.SelectSingleNode("./div/a");

                    var anime = new AnimeModel()
                    {
                        Image = aNode.SelectSingleNode("./img").GetAttributeValue("src", ""),
                        Url = aNode.GetAttributeValue("href", ""),
                        Title = aNode.GetAttributeValue("title", ""),
                    };
                    animeList.Add(anime);
                }

                currentPage++;
                doc = web.Load($"{requestUrl}page={currentPage}");
                paginationNode = doc.GetElementbyId("pagination");
            }

            AnimeSaturnUtilities.FillAnimeList(animeList);

            return animeList.ToArray();
        }

        public void Download(string downloadPath, Uri animeUrl)
        {
            var web = new HtmlWeb();
            var doc = web.Load(animeUrl);

            var source = doc
                .GetElementbyId("myvideo_html5_api")
                .SelectSingleNode("//source")
                .InnerText;

            DownlaClient downlaClient = new DownlaClient(downloadPath);

            var cts = new CancellationTokenSource();

            downlaClient.DownloadAsync(new Uri(source), cts.Token);
            downlaClient.EnsureDownload(cts.Token);
            
        }
    }
}
