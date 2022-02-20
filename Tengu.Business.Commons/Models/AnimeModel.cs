using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Commons
{
    public class AnimeModel
    {
        public string Id { get; set; } = string.Empty;
        public Hosts Host { get; set; } = Hosts.None;
        public string Url { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public KitsuAnimeModel? KitsuAttributes { get; set; }

    }
}
