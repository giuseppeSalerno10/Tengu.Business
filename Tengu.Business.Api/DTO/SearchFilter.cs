using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public class SearchFilter
    {
        public IEnumerable<Genres> Genres { get; set; } = Array.Empty<Genres>();
        public IEnumerable<string> Years { get; set; } = Array.Empty<string>();
        public IEnumerable<Statuses> Statuses { get; set; } = Array.Empty<Statuses>();
        public IEnumerable<Languages> Languages { get; set; } = Array.Empty<Languages>();
    }
}