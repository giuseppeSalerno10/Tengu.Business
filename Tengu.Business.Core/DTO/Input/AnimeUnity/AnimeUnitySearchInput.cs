using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
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
