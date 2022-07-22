using Tengu.Business.Commons.Objects;

namespace Tengu.Business.Commons.Models
{
    public class AnimeModel
    {
        private string title = string.Empty;

        public string Id { get; set; } = string.Empty;
        public TenguHosts Host { get; set; } = TenguHosts.None;
        public string Url { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Title { get => title; set => title = value.Normalize(); }
        public KitsuAnimeModel? KitsuAttributes { get; set; }

    }
}
