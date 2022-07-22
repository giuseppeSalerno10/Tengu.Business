﻿using Flurl.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.Adapters.Interfaces;
using Tengu.Business.Core.DTO.Input.AnimeUnity;
using Tengu.Business.Core.DTO.Output.AnimeUnity;
using Tengu.Business.Core.Utilities.Interfaces;

namespace Tengu.Business.Core.Adapters
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
            var requestUrl = $"{Config.AnimeUnityConfig.BaseEpisodeUrl}/{animeId}";

            var episodeList = new List<EpisodeModel>();

            var startIndex = 1;
            var endIndex = 120;
            var episodeCount = 0;

            while (episodeCount == 0 || startIndex < episodeCount)
            {
                var response = await $"{requestUrl}/1?start_range={startIndex}&end_range={endIndex}"
                    .GetJsonAsync<AnimeUnityGetEpisodesOutput>();

                episodeCount = response.Episodes_count;

                foreach (var episode in response.Episodes)
                {

                    string downloadUrl = await _utilities.GetDownloadUrl(episode.Scws_id, episode.File_name);

                    var episodeToAdd = new EpisodeModel()
                    {
                        Url = $"{Config.AnimeUnityConfig.BaseAnimeUrl}/{animeId}-{response.Slug}",
                        Title = response.Name,
                        AnimeId = animeId,
                        DownloadUrl = episode.Link,
                        EpisodeNumber = episode.Number,
                        Host = TenguHosts.AnimeUnity,
                        Id = episode.Id.ToString(),
                    };

                    episodeList.Add(episodeToAdd);
                }

                startIndex = endIndex + 1;
                endIndex += 120;
            }

            return episodeList.ToArray();
        }

        public async Task<EpisodeModel[]> GetLatestEpisodesAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var episodeList = new List<EpisodeModel>();

            var requestUrl = $"{Config.AnimeUnityConfig.BaseUrl}";

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
                        Host = TenguHosts.AnimeUnity,
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

            var requestUrl = Config.AnimeUnityConfig.SearchUrl;

            var session = await _utilities.CreateSession();

            AnimeUnitySearchOutput response;

            do
            {
                response = await requestUrl
                .WithHeader("X-Requested-With", "XMLHttpRequest")
                .WithHeader("X-XSRF-TOKEN", session.XSRFToken)
                .WithHeader("User-Agent", Config.HttpConfig.UserAgent)
                .WithCookie("XSRF-TOKEN", session.XSRFCookieToken)
                .WithCookie("animeunity_session", session.AnimeUnitySession)
                .PostJsonAsync(searchFilter)
                .ReceiveJson<AnimeUnitySearchOutput>();

                foreach (var record in response.Records)
                {
                    if (animeList.Count < count)
                    {
                        var anime = new AnimeModel()
                        {
                            Host = TenguHosts.AnimeUnity,
                            Id = $"{record.Id}",
                            Image = record.ImageUrl,
                            Title = record.Title,
                            Url = $"{Config.AnimeUnityConfig.BaseAnimeUrl}/{record.Id}-{record.Slug}"
                        };
                        animeList.Add(anime);
                    }
                }

                searchFilter.Offset += 30;
            } while (animeList.Count < count && response.Records.Length > 0);

            return animeList.ToArray();
        }

        public async Task<Calendar> GetCalendar(CancellationToken cancellationToken = default)
        {
            var calendar = new Calendar();

            var requestUrl = $"{Config.AnimeUnityConfig.CalendarUrl}";

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
                    "Lunedì" => TenguWeekDays.Monday,
                    "Martedì" => TenguWeekDays.Tuesday,
                    "Mercoledì" => TenguWeekDays.Wednesday,
                    "Giovedì" => TenguWeekDays.Thursday,
                    "Venerdì" => TenguWeekDays.Friday,
                    "Sabato" => TenguWeekDays.Saturday,
                    "Domenica" => TenguWeekDays.Sunday,
                    _ => TenguWeekDays.None
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

        public async Task<string> GetStreamUrl(string episodeUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}
