using Tengu.Business.Commons.Objects;

namespace Tengu.Business.API.DTO
{
    public class SearchFilter
    {
        public IEnumerable<Genres> Genres { get; set; } = Array.Empty<Genres>();
        public string Year { get; set; } = string.Empty;
        public Statuses Status { get; set; } = Statuses.None;
    }
}