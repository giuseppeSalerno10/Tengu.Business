using Tengu.Business.Commons;

namespace Tengu.Business.Core.DTO.Output.AnimeSaturn
{
    internal class AnimeSaturnSearchTitleOutput
    {
        private string link = string.Empty;

        public string Link
        {
            get => link;
            set => link = $"{Config.AnimeSaturn.BaseAnimeUrl}/{value}";
        }
        public string Image { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Release { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
