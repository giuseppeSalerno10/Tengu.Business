using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public class SearchFilter
    {
        public IEnumerable<Genres> Genres { get; set; } = Array.Empty<Genres>();
        public string Year { get; set; } = string.Empty;
        public Statuses Status { get; set; } = Statuses.None;
    }
}