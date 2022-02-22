using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Core
{
    public class AnimeUnityGetLatestEpisodesOutput
    {
        public AnimeUnityGetEpisodesOutput[] Data { get; set; } = Array.Empty<AnimeUnityGetEpisodesOutput>();
        public string Next_page_url { get; set; } = string.Empty;
    }
}
