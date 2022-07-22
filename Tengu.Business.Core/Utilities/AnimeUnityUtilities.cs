using Flurl.Http;
using System.Security.Cryptography;
using System.Text;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.DTO.Output.AnimeUnity;
using Tengu.Business.Core.DTO.Output.AnimeUnity.Object;
using Tengu.Business.Core.Utilities.Interfaces;

namespace Tengu.Business.Core.Utilities
{
    public class AnimeUnityUtilities : IAnimeUnityUtilities
    {
        public async Task<string> GetDownloadUrl(string scwsId, string fileName)
        {
            dynamic scwsResponse = await $"https://scws.xyz/videos/{scwsId}".GetJsonAsync<dynamic>();
            string ipResponse = await "https://au-a1-01.scws-content.net/get-ip".GetStringAsync();

            var token = CryptString(2, ipResponse, "Yc8U6r8KjAKAepEA");

            string downloadUrl= $"https://au-d1-0{scwsResponse.proxy_download}.scws-content.net/download/{scwsResponse.storage_download.number}/{scwsResponse.folder_id}/{scwsResponse.quality}p.mp4?token={token}&filename={fileName}";
            return downloadUrl;
        }


        public async Task<AnimeUnityCreateSessionOutput> CreateSession()
        {
            var requestUrl = Config.AnimeUnityConfig.BaseUrl;

            var headResponse = await requestUrl
                .WithHeader("User-Agent", Config.HttpConfig.UserAgent)
                .HeadAsync();

            var xsrfToken = headResponse.Cookies.First(cookie => cookie.Name.Equals("XSRF-TOKEN")).Value;
            var animeUnitySession = headResponse.Cookies.First(cookie => cookie.Name.Equals("animeunity_session")).Value;


            var sessionResult = new AnimeUnityCreateSessionOutput()
            {
                XSRFToken = xsrfToken.Replace("%3D", "="),
                XSRFCookieToken = xsrfToken,
                AnimeUnitySession = animeUnitySession
            };

            return sessionResult;
        }

        public AnimeUnityGenre[] GetGenreArray(IEnumerable<TenguGenres> genres)
        {
            var genreMap = new Dictionary<TenguGenres, AnimeUnityGenre>()
            {
                { TenguGenres.Azione, new  AnimeUnityGenre { Id = 51, Name = "Action" } },
                { TenguGenres.Avventura, new  AnimeUnityGenre { Id = 21, Name =  "Adventure" } },
                { TenguGenres.Veicoli, new  AnimeUnityGenre { Id = 29, Name = "Cars" } },
                { TenguGenres.Commedia, new  AnimeUnityGenre { Id = 37, Name = "Comedy" } },
                { TenguGenres.Demenziale, new  AnimeUnityGenre { Id = 43, Name = "Dementia" } },
                { TenguGenres.Demoni, new  AnimeUnityGenre { Id = 13, Name = "Demons" } },
                { TenguGenres.Drammatico, new  AnimeUnityGenre { Id = 22, Name = "Drama" } },
                { TenguGenres.Ecchi, new  AnimeUnityGenre { Id = 5, Name = "Ecchi" } },
                { TenguGenres.Fantasy, new  AnimeUnityGenre { Id = 9, Name = "Fantasy" } },
                { TenguGenres.Gioco, new  AnimeUnityGenre { Id = 44, Name = "Game" } },
                { TenguGenres.Harem, new  AnimeUnityGenre { Id = 15, Name = "Harem" } },
                { TenguGenres.Hentai, new  AnimeUnityGenre { Id = 4, Name = "Hentai" } },
                { TenguGenres.Storico, new  AnimeUnityGenre { Id = 30, Name = "Historical" } },
                { TenguGenres.Horror, new  AnimeUnityGenre { Id = 3, Name = "Horror" } },
                { TenguGenres.Josei, new  AnimeUnityGenre { Id = 45, Name = "Josei" } },
                { TenguGenres.Bambini, new  AnimeUnityGenre { Id = 14, Name = "Kids" } },
                { TenguGenres.Magia, new  AnimeUnityGenre { Id = 23, Name = "Magic" } },
                { TenguGenres.ArtiMarziali, new  AnimeUnityGenre { Id = 31, Name = "Martial Arts" } },
                { TenguGenres.Mecha, new  AnimeUnityGenre { Id = 38, Name = "Mecha" } },
                { TenguGenres.Militari, new  AnimeUnityGenre { Id = 46, Name = "Military" } },
                { TenguGenres.Mistero, new  AnimeUnityGenre { Id = 24, Name = "Mistery" } },
                { TenguGenres.Musicale, new  AnimeUnityGenre { Id = 16, Name = "Music" } },
                { TenguGenres.Parodia, new  AnimeUnityGenre { Id = 32, Name = "Parody" } },
                { TenguGenres.Polizia, new  AnimeUnityGenre { Id = 39, Name = "Police" } },
                { TenguGenres.Psicologico, new  AnimeUnityGenre { Id = 47, Name = "Psychological" } },
                { TenguGenres.Romantico, new  AnimeUnityGenre { Id = 17, Name = "Romance" } },
                { TenguGenres.Samurai, new  AnimeUnityGenre { Id = 25, Name = "Samurai" } },
                { TenguGenres.Scolastico, new  AnimeUnityGenre { Id = 33, Name = "School" } },
                { TenguGenres.SciFi, new  AnimeUnityGenre { Id = 40, Name = "Sci - fi" } },
                { TenguGenres.Seinen, new  AnimeUnityGenre { Id = 49, Name = "Seinen" } },
                { TenguGenres.Shoujo, new  AnimeUnityGenre { Id = 18, Name = "Shoujo" } },
                { TenguGenres.ShoujoAi, new  AnimeUnityGenre { Id = 26, Name = "Shoujo Ai" } },
                { TenguGenres.Shounen, new  AnimeUnityGenre { Id = 34, Name = "Shounen" } },
                { TenguGenres.ShounenAi, new  AnimeUnityGenre { Id = 41, Name = "Shounen Ai" } },
                { TenguGenres.SliceOfLife, new  AnimeUnityGenre { Id = 50, Name = "Slice of Life" } },
                { TenguGenres.Spazio, new  AnimeUnityGenre { Id = 19, Name = "Space" } },
                { TenguGenres.Sport, new  AnimeUnityGenre { Id = 27, Name = "Sports" } },
                { TenguGenres.Superpoteri, new  AnimeUnityGenre { Id = 35, Name = "Super Power" } },
                { TenguGenres.Soprannaturale, new  AnimeUnityGenre { Id = 42, Name = "Supernatural" } },
                { TenguGenres.Thriller, new  AnimeUnityGenre { Id = 48, Name = "Thriller" } },
                { TenguGenres.Vampiri, new  AnimeUnityGenre { Id = 20, Name = "Vampire" } },
                { TenguGenres.Yaoi, new  AnimeUnityGenre { Id = 28, Name = "Yaoi" } },
                { TenguGenres.Yuri, new  AnimeUnityGenre { Id = 36, Name = "Yuri" } },
            };

            var genresList = new List<AnimeUnityGenre>();

            foreach (var genre in genres)
            {
                AnimeUnityGenre? mappedGenre = genreMap.GetValueOrDefault(genre);

                if (mappedGenre != null)
                {
                    genresList.Add(mappedGenre);
                }
            }

            return genresList.ToArray();
        }

        public string GetStatus(TenguStatuses status)
        {
            Dictionary<TenguStatuses, string> statusMap = new Dictionary<TenguStatuses, string>()
            {
                { TenguStatuses.InProgress, "In Corso"},
                { TenguStatuses.Completed, "Terminato"},
                { TenguStatuses.NotReleased, "In Uscita"},
                { TenguStatuses.Canceled, "Droppato"},
            };

            return statusMap[status];
        }

        private string CryptString(int coeff, string clearData, string key)
        {
            /*
                var r = new Date(Date.now() + 36e5 * t).getTime()
                i = (r = String(Math.round(r / 1e3))) + e + " " + n;
                return CryptoJS.MD5(i).toString(CryptoJS.enc.Base64).replace(/=/ g, "").replace(/\+/ g, "-").replace(/\//g, "_") + "&expires=" + r

                    n = key
                    e = clearData
                    coeff = t
            */
            long currentMillis = (long)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 36e5 * coeff);
            long expireTime = Convert.ToInt64(currentMillis / 1000);

            string strToCrypt = $"{expireTime}{clearData} {key}";
            var encodedStr = Encoding.ASCII.GetBytes(strToCrypt);

            byte[] cryptedData = MD5.HashData(encodedStr);
            string rawCryptedString = Convert.ToBase64String(cryptedData);

            string cryptedString = rawCryptedString
                .Replace("=", "")
                .Replace("+", "-")
                .Replace("/", "_");

            return $"{cryptedString}&expires={expireTime}";
        }
    }
}
