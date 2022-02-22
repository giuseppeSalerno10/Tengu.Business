using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Core
{
    public class AnimeUnitySearchOutput
    {
        public AnimeUnitySearchTitleRecord[] Records { get; set; } = Array.Empty<AnimeUnitySearchTitleRecord>();
    }

    public class AnimeUnitySearchTitleRecord
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Plot { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Title_eng { get; set; } = string.Empty;
        public string Studio { get; set; } = string.Empty;
    }
}
