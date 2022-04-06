using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Core
{
    public class AnimeUnityGetLatestEpisodesOutput
    {
        public AnimeUnityEpisodesInfo[] Data { get; set; } = Array.Empty<AnimeUnityEpisodesInfo>();
        public string Next_page_url { get; set; } = string.Empty;
    }
}
