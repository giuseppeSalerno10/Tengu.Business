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
        public List<string> Tags { get; set; } = new List<string>();
        public List<EpisodeModel> Episodes { get; set; } = new List<EpisodeModel>();
        public int TotalEpisode { get; set; }
    }
}
