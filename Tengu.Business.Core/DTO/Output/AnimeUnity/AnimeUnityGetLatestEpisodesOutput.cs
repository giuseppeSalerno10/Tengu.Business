using Tengu.Business.Core.DTO.Output.AnimeUnity.Object;

namespace Tengu.Business.Core.DTO.Output.AnimeUnity
{
    public class AnimeUnityGetLatestEpisodesOutput
    {
        public AnimeUnityEpisodesInfo[] Data { get; set; } = Array.Empty<AnimeUnityEpisodesInfo>();
        public string Next_page_url { get; set; } = string.Empty;
    }
}
