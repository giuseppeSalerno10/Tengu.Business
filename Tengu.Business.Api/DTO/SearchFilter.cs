using Tengu.Business.Commons.Objects;

namespace Tengu.Business.API.DTO
{
    public class TenguSearchFilter
    {
        public IEnumerable<TenguGenres> Genres { get; set; } = Array.Empty<TenguGenres>();
        public string Year { get; set; } = string.Empty;
        public TenguStatuses Status { get; set; } = TenguStatuses.None;
    }
}