using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public static class AnimeSaturnUtilities
    {
        public static void FillAnimeList(IEnumerable<AnimeModel> animeList)
        {
            var web = new HtmlWeb();
            HtmlDocument doc;

            foreach (var anime in animeList)
            {
                doc = web.Load($"{anime.Url}");

                var alternativeTitle = doc.DocumentNode.SelectSingleNode("./" +
                    "div[@class='container mt-3']/" +
                    "div[@class='row altezza-row-anime']/" +
                    "div[@class='col-md-9 pr-0 mobile-padding margin-anime-page']/" +
                    "div[@class='container anime-title-as mb-3 w-100']" +
                    "div[@class='box-trasparente-alternativo rounded'")
                    .InnerText;

                var episodesNode = doc
                    .GetElementbyId("resultsxd")
                    .SelectNodes("./div/div/div");

                foreach (var node in episodesNode)
                {
                    var aNode = node.SelectSingleNode("./a");
                    var episode = new EpisodeModel
                    {
                        Url = aNode.GetAttributeValue("src", ""),
                        Title = aNode.InnerText
                    };

                    anime.Episodes.Add(episode);
                }
            }
        }
    }
}
