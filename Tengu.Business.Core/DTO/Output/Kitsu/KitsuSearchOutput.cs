using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Business.Core
{
    public class KitsuSearchOutput
    {
        public KitsuData[] Data { get; set; } = Array.Empty<KitsuData>();
    }

    public class KitsuData
    { 
        public KintsuLinks Links { get; set; } = new KintsuLinks();
        public KintsuAttribute Attributes { get; set; } = new KintsuAttribute();
    }

    public class KintsuAttribute
    {
        public string? Synopsis { get; set; }
        public string? CanonicalTitle { get; set; }
        public string? AverageRating { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int? PopularityRank { get; set; }
        public int? RatingRank { get; set; }
        public string? AgeRating { get; set; }
        public int? EpisodeCount { get; set; }
        public KitsuImage? PosterImages { get; set; }

    }

    public class KitsuImage
    {
        public string? Small { get; set; }
    }

    public class KintsuLinks
    {
        public string? Self { get; set; }
    }

}
