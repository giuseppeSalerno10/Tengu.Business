using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public class AnimeUnityUtilities : IAnimeUnityUtilities
    {
        public async Task<AnimeModel[]> FillAnimeList(IEnumerable<AnimeModel> animeList, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public string[] GetGenreArray(IEnumerable<Genres> genres)
        {
            throw new NotImplementedException();
        }

        public string[] GetLanguagesArray(IEnumerable<Languages> languages)
        {
            throw new NotImplementedException();
        }

        public string[] GetStatusesArray(IEnumerable<Statuses> statuses)
        {
            throw new NotImplementedException();
        }
    }
}
