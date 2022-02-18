using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public class AnimeUnityAdapter : IAnimeUnityAdapter
    {
        public IEnumerable<AnimeModel> SearchByTitle(string title)
        {
            var requestUrl = $"{Config.AnimeUnity.SearchByTitleUrl}";

            var requestBody = new AnimeUnitySearchTitleInput()
            {
                Title = title,
            };

            var response = requestUrl
                .PostJsonAsync(requestBody)
                .ReceiveJson<AnimeUnitySearchTitleOutput[]>()
                .Result;

            var aniemeList = new List<AnimeModel>();

            foreach (var item in response)
            {
                var anime = new AnimeModel()
                {
                    Title = item.Name,
                    Image = item.Image,
                    Url = item.Link
                };

                aniemeList.Add(anime);
            }

            //AnimeSaturnUtilities.FillAnimeList(aniemeList);

            return aniemeList;
        }

        public IEnumerable<AnimeModel> SearchByFilters()
        {
            throw new NotImplementedException();
        }

        public void Download(string downloadPath, Uri uri)
        {

        }
    }
}
