using Newtonsoft.Json;

namespace Tengu.Business.Core.DTO.Output.AnimeUnity.Object
{
    public class AnimeUnityGenre
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;
    }
}
