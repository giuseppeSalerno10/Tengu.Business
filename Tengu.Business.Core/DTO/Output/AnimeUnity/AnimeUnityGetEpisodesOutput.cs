using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Core
{
    public class AnimeUnityGetEpisodesOutput
    {
        public int Episodes_count { get; set; }
        public AnimeUnityEpisodesInfo[] Episodes { get; set; } = Array.Empty<AnimeUnityEpisodesInfo>();
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
