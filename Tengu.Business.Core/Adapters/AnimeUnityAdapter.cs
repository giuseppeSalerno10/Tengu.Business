using Flurl.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
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

        public AnimeUnityAdapter(IAnimeUnityUtilities utilities)
        {
            _utilities = utilities;
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
                    Url = requestUrl,
                    Title = $"Episode {episode.Number}",
                    AnimeId = animeId,
                    DownloadUrl = episode.Link,
                    EpisodeNumber = episode.Number,
                    Host = Hosts.AnimeUnity,
                    Id = episode.Id.ToString(),
                };

                episodeList.Add(episodeToAdd);
            }

            return episodeList.ToArray();
        }

        public async Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var episodeList = new List<EpisodeModel>();

            var requestUrl = $"{Config.AnimeUnity.BaseUrl}";

            var currentPage = offset / 30;

            requestUrl += $"/?page={currentPage + 1}";

            var web = new HtmlWeb();

            var count = limit - offset;

            while (!cancellationToken.IsCancellationRequested && count > 0)
            {
                var startIndex = offset - currentPage * 30;
                var endIndex = Math.Min(30, startIndex + count);

                var doc = await web.LoadFromWebAsync(requestUrl, cancellationToken);

                var episodesJson = doc.DocumentNode
                    .SelectSingleNode("//layout-items")
                    .GetAttributeValue("items-json", "")
                    .Replace("&quot;", "\"")
                    .Replace("\\/", "/");

                var rawEpisodes = JsonConvert.DeserializeObject<AnimeUnityGetLatestEpisodesOutput>(episodesJson);
                
                for (int i = startIndex; i < endIndex; i++)
                {
                    var episode = rawEpisodes.Data[i];

                    var episodeToAdd = new EpisodeModel()
                    {
                        Title = $"{episode.Anime.Title}",
                        AnimeId = episode.Anime.Id.ToString(),
                        EpisodeNumber = episode.Number,
                        Host = Hosts.AnimeUnity,
                        Id = episode.Id.ToString(),
                        DownloadUrl = episode.Link,
                        Image = episode.Anime.ImageUrl
                    };
                
                    episodeList.Add(episodeToAdd);
                }
            
                requestUrl = rawEpisodes.Next_page_url;
            
                count -= endIndex - startIndex;
                offset = ++currentPage * 30;
            
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
                .WithHeader("X-Requested-With", "XMLHttpRequest")
                .WithHeader("X-XSRF-TOKEN", session.XSRFToken)
                .WithHeader("User-Agent", Config.Common.UserAgent)
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

        public async Task<Calendar> GetCalendar(CancellationToken cancellationToken = default)
        {
            var calendar = new Calendar();

            var requestUrl = $"{Config.AnimeUnity.CalendarUrl}";

            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(requestUrl, cancellationToken);

            var calendarNodes = doc.DocumentNode.SelectNodes("//calendario-item");

            foreach (var node in calendarNodes)
            {
                var rawJson = node.GetAttributeValue("a", "")
                .Replace("&quot;", "\"")
                .Replace("\\/", "/"); ;

                var rawCalendarEntry = JsonConvert.DeserializeObject<AnimeUnityCalendarOutput>(rawJson);

                var day = rawCalendarEntry.Day switch
                {
                    "Lunedì" => WeekDays.Monday,
                    "Martedì" => WeekDays.Tuesday,
                    "Mercoledì" => WeekDays.Wednesday,
                    "Giovedì" => WeekDays.Thursday,
                    "Venerdì" => WeekDays.Friday,
                    "Sabato" => WeekDays.Saturday,
                    "Domenica" => WeekDays.Sunday,
                    _ => WeekDays.None
                };

                var entryToAdd = new CalendarEntryModel()
                {
                    Anime = rawCalendarEntry.Title,
                    Image = rawCalendarEntry.ImageURL
                };

                calendar.DaysDictionary[day].Add(entryToAdd);
            }

            

            return calendar;
        }
    }
}
