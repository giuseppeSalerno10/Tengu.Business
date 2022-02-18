using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public class KitsuAdapter : IKitsuAdapter
    {
        public async Task<AnimeModel[]> SearchAnime(string title)
        {
            var response = $"{Config.Kitsu.BaseUrl}filter[text]={title}"
                .WithHeader("Accept", "application/vnd.api+json")
                .WithHeader("Content-Type", "application/vnd.api+json")
                .GetJsonAsync();

            throw new NotImplementedException();
        }
    }
}
