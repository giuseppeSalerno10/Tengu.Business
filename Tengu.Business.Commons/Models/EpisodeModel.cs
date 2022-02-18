using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Commons
{
    public class EpisodeModel
    {
        public Hosts Host { get; set; } = Hosts.None;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}
