using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Commons
{
    public class AnimeModel
    {

        public string Url { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string AlternativeTitle { get; set; } = string.Empty;

        public string Studio { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public EpisodeModel[] Episodes { get; set; } = Array.Empty<EpisodeModel>();
        public int TotalEpisode { get; set; }
    }
}
