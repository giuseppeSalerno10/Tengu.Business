using Tengu.Business.Commons.Objects;

namespace Tengu.Business.Commons.Models
{
    public class EpisodeModel : IComparable<EpisodeModel>
    {
        public string Id { get; set; } = string.Empty;
        public string AnimeId { get; set; } = string.Empty;
        public Hosts Host { get; set; } = Hosts.None;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string EpisodeNumber { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;

        public int CompareTo(EpisodeModel? other)
        {
            var currentEpisode = Convert.ToInt32(EpisodeNumber);
            var otherEpisode = Convert.ToInt32(other?.EpisodeNumber);

            return currentEpisode.CompareTo(otherEpisode);
        }
    }
}
