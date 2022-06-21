using Tengu.Business.Core.DTO.Output.AnimeUnity.Object;

namespace Tengu.Business.Core.DTO.Output.AnimeUnity
{
    public class AnimeUnityGetEpisodesOutput
    {
        public int Episodes_count { get; set; }
        public AnimeUnityEpisodesInfo[] Episodes { get; set; } = Array.Empty<AnimeUnityEpisodesInfo>();
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
