﻿using Tengu.Business.Commons.Objects;

namespace Tengu.Business.Commons.Models
{
    public class EpisodeModel : IComparable<EpisodeModel>
    {
        private string title = string.Empty;

        public string Id { get; set; } = string.Empty;
        public string AnimeId { get; set; } = string.Empty;
        public TenguHosts Host { get; set; } = TenguHosts.None;
        public string Title { get => title; set => title = value.Normalize(); }
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
