using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Core
{
    public class AnimeUnityEpisodesInfo
    {
        public int Id { get; set; }
        public AnimeUnityAnime Anime { get; set; } = new AnimeUnityAnime();
        public string Number { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Scws_id { get; set; } = string.Empty;
        public string File_name { get; set; } = string.Empty;
    }
}
