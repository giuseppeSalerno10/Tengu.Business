namespace Tengu.Business.Core.DTO.Input.AnimeSaturn
{
    public class AnimeSaturnSearchFilterInput
    {
        public IEnumerable<string> Genres { get; set; } = Array.Empty<string>();
        public string Year { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
