namespace Tengu.Business.Commons.Models
{
    public class KitsuAnimeModel
    {
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int TotalEpisodes { get; set; }
        public string ReleaseDate { get; set; } = string.Empty;
        public string KitsuUrl { get; set; } = string.Empty;
        public string AgeRating { get; set; } = string.Empty;
        public int RatingRank { get; set; }
        public int PopularityRank { get; set; }
        public string AverageRating { get; set; } = string.Empty;
        public string Synopsis { get; set; } = string.Empty;
    }
}
