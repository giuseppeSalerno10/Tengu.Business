using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public static class AnimeSaturnUtilities
    {
        public async static Task<AnimeModel[]> FillAnimeList(IEnumerable<AnimeModel> animeList)
        {
            var concurrentAnimeList = new ConcurrentBag<AnimeModel>();

            foreach (var anime in animeList)
            {
                concurrentAnimeList.Add(anime);
            }

            var ct = CancellationToken.None;

            await Parallel.ForEachAsync(concurrentAnimeList, ct, async (anime, ct) => 
            {
                var web = new HtmlWeb();
                HtmlDocument doc;

                doc = await web.LoadFromWebAsync($"{anime.Url}");

                anime.AlternativeTitle = doc.DocumentNode.SelectSingleNode("./div/div/div/div/div[@class='box-trasparente-alternativo rounded']")
                    .InnerText;

                var episodesNode = doc
                    .GetElementbyId("resultsxd")
                    .SelectNodes("./div/div/div");

                var episodeList = new List<EpisodeModel>();

                foreach (var node in episodesNode)
                {
                    var internalUrl = node.SelectSingleNode("./a").GetAttributeValue("href", "");
                    var internalWeb = new HtmlWeb();
                    var internalDoc = internalWeb.Load(internalUrl);

                    var episode = new EpisodeModel
                    {
                        Title = internalDoc.DocumentNode
                            .SelectSingleNode("./div/div/div[@class='card-body']/h3[@class='text-center mb-4']")
                            .InnerText
                            .Split("<br>")[^1]
                            .Trim(),
                        Url = internalDoc.DocumentNode
                            .SelectSingleNode("./div/div/div[@class='card-body']/a")
                            .GetAttributeValue("href", ""),
                        Image = anime.Image
                    };
                    episodeList.Add(episode);
                }

                anime.Episodes = episodeList.ToArray();

            });

            return concurrentAnimeList.ToArray();
        }
    }
}
