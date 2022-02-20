using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public class KitsuAdapter : IKitsuAdapter
    {
        public async Task<KitsuAnimeModel[]> GetUpcomingAnime(int offset, int limit, CancellationToken cancellationToken = default)
        {
            var response = await $"{Config.Kitsu.BaseUrl}filter[status]=upcoming&page[limit]={limit}&page[offset]={offset}"
                .WithHeader("Accept", "application/vnd.api+json")
                .WithHeader("Content-Type", "application/vnd.api+json")
                .GetJsonAsync<KitsuSearchOutput>(cancellationToken);

            var animeList = new List<KitsuAnimeModel>();

            foreach (var item in response.Data)
            {
                animeList.Add(new KitsuAnimeModel()
                {
                    KitsuUrl = item.Links.Self ?? "",
                    ReleaseDate = item.Attributes.StartDate ?? "",
                    TotalEpisodes = item.Attributes.EpisodeCount ?? 0,
                    AgeRating = item.Attributes.AgeRating ?? "",
                    RatingRank = item.Attributes.RatingRank ?? 0,
                    PopularityRank = item.Attributes.PopularityRank ?? 0,
                    AverageRating = item.Attributes.AverageRating ?? "",
                    Synopsis = item.Attributes.Synopsis ?? "",
                    Title = item.Attributes.CanonicalTitle ?? "",
                });
            }
            return animeList.ToArray();
        }

        public async Task<KitsuAnimeModel[]> SearchAnime(string title, int limit, CancellationToken cancellationToken = default)
        {
            var response = await $"{Config.Kitsu.BaseUrl}filter[text]={title}&page[limit]={limit}"
                .WithHeader("Accept", "application/vnd.api+json")
                .WithHeader("Content-Type", "application/vnd.api+json")
                .GetJsonAsync<KitsuSearchOutput>(cancellationToken);

            var animeList = new List<KitsuAnimeModel>();

            foreach (var item in response.Data)
            {
                animeList.Add(new KitsuAnimeModel()
                {
                    KitsuUrl = item.Links.Self ?? "",
                    ReleaseDate = item.Attributes.StartDate ?? "",
                    TotalEpisodes = item.Attributes.EpisodeCount ?? 0,
                    AgeRating = item.Attributes.AgeRating ?? "",
                    RatingRank = item.Attributes.RatingRank ?? 0,
                    PopularityRank = item.Attributes.PopularityRank ?? 0,
                    AverageRating = item.Attributes.AverageRating ?? "",
                    Synopsis = item.Attributes.Synopsis ?? "",
                    Title = item.Attributes.CanonicalTitle ?? "",
                });
            }
            return animeList.ToArray();
        }
    }
}
