using Flurl.Http;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;
using Tengu.Business.Core.Adapters.Interfaces;
using Tengu.Business.Core.DTO.Output.Kitsu;

namespace Tengu.Business.Core.Adapters
{
    public class KitsuAdapter : IKitsuAdapter
    {
        public async Task<KitsuAnimeModel[]> GetUpcomingAnimeAsync(int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var animeList = new List<KitsuAnimeModel>();

            var lowerBound = offset;
            var upperBound = Math.Min(limit - offset, Config.Kitsu.MaxAnimes);

            while (upperBound > 0)
            {
                var requestUrl = $"{Config.Kitsu.BaseUrl}filter[status]=upcoming&" +
                    $"page[offset]={lowerBound}&" +
                    $"page[limit]={upperBound}&" +
                    $"sort=popularityRank";

                var response = await requestUrl
                    .WithHeader("Accept", "application/vnd.api+json")
                    .WithHeader("Content-Type", "application/vnd.api+json")
                    .GetJsonAsync<KitsuSearchOutput>(cancellationToken);

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
                        Image = item.Attributes.PosterImage?.Small ?? ""
                    });
                }

                lowerBound = lowerBound + upperBound + 1;
                upperBound = Math.Min(limit - lowerBound + 1, Config.Kitsu.MaxAnimes);
            }


            return animeList.ToArray();
        }

        public async Task<KitsuAnimeModel[]> SearchAnimeAsync(string title, int offset = 0, int limit = 30, CancellationToken cancellationToken = default)
        {
            var animeList = new List<KitsuAnimeModel>();

            var lowerBound = offset;
            var upperBound = Math.Min(limit - offset, Config.Kitsu.MaxAnimes);

            while (upperBound > 0)
            {

                var requestUrl = $"{Config.Kitsu.BaseUrl}filter[text]={title}&" +
                    $"page[offset]={lowerBound}&" +
                    $"page[limit]={upperBound}&";

                var response = await requestUrl
                    .WithHeader("Accept", "application/vnd.api+json")
                    .WithHeader("Content-Type", "application/vnd.api+json")
                    .GetJsonAsync<KitsuSearchOutput>(cancellationToken);

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
                        Image = item.Attributes.PosterImage?.Small ?? ""
                    });
                }

                lowerBound = lowerBound + upperBound + 1;
                upperBound = Math.Min(limit - lowerBound + 1, Config.Kitsu.MaxAnimes);
            }

            return animeList.ToArray();
        }
    }
}
