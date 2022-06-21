using Newtonsoft.Json;
using Tengu.Business.Core.DTO.Output.AnimeUnity.Object;

namespace Tengu.Business.Core.DTO.Input.AnimeUnity
{
    public class AnimeUnitySearchInput
    {
        [JsonProperty(PropertyName = "title")]
        public string? Title { get; set; } = null;

        [JsonProperty(PropertyName = "type")]
        public string? Type { get; set; } = null;

        [JsonProperty(PropertyName = "year")]
        public string? Year { get; set; } = null;

        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; } = "Popolarità";

        [JsonProperty(PropertyName = "status")]
        public string? Status { get; set; } = null;

        [JsonProperty(PropertyName = "genres")]
        public AnimeUnityGenre[]? Genres { get; set; } = null;

        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; } = 0;

    }
}
