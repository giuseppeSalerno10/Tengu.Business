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
    public class AnimeSaturnUtilities : IAnimeSaturnUtilities
    {
        public async Task<AnimeModel[]> FillAnimeList(IEnumerable<AnimeModel> animeList, CancellationToken cancellationToken = default)
        {
            var concurrentAnimeList = new ConcurrentBag<AnimeModel>();

            foreach (var anime in animeList)
            {
                concurrentAnimeList.Add(anime);
            }

            await Parallel.ForEachAsync(concurrentAnimeList, cancellationToken, async (anime, cancellationToken) =>
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

        public string[] GetGenreArray(IEnumerable<Genres> genres)
        {
            Dictionary<Genres, string> genreMap = new Dictionary<Genres, string>()
            {
                { Genres.Martial, "Arti Marziali" },
                { Genres.Adventure, "Avventura" }
            };

            var genresList = new List<string>();

            foreach (var genre in genres)
            {
                genresList.Add(genreMap[genre]);
            }

            return genresList.ToArray();
        }
        public string[] GetStatusesArray(IEnumerable<Statuses> statuses)
        {
            Dictionary<Statuses, string> statusMap = new Dictionary<Statuses, string>()
            {
                { Statuses.InProgress, "0"},
                { Statuses.Completed, "1"},
                { Statuses.NotReleased, "2"},
                { Statuses.Canceled, "3"},
            };

            var statusesList = new List<string>();

            foreach (var status in statuses)
            {
                statusesList.Add(statusMap[status]);
            }

            return statusesList.ToArray();
        }
        public string[] GetLanguagesArray(IEnumerable<Languages> languages)
        {
            Dictionary<Languages, string> langMap = new Dictionary<Languages, string>()
            {
                { Languages.Dubbed, "1" },
                { Languages.Subbed , "0" }
            };

            var languagesList = new List<string>();

            foreach (var status in languages)
            {
                languagesList.Add(langMap[status]);
            }

            return languagesList.ToArray();
        }
    }
}
