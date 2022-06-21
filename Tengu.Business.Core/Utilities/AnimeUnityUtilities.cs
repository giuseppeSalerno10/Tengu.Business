using Flurl.Http;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.DTO.Output.AnimeUnity;
using Tengu.Business.Core.DTO.Output.AnimeUnity.Object;
using Tengu.Business.Core.Utilities.Interfaces;

namespace Tengu.Business.Core.Utilities
{
    public class AnimeUnityUtilities : IAnimeUnityUtilities
    {
        public async Task<AnimeUnityCreateSessionOutput> CreateSession()
        {
            var requestUrl = Config.AnimeUnity.BaseUrl;

            var headResponse = await requestUrl
                .WithHeader("User-Agent", Config.Common.UserAgent)
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

        public AnimeUnityGenre[] GetGenreArray(IEnumerable<Genres> genres)
        {
            var genreMap = new Dictionary<Genres, AnimeUnityGenre>()
            {
                { Genres.Azione, new  AnimeUnityGenre { Id = 51, Name = "Action" } },
                { Genres.Avventura, new  AnimeUnityGenre { Id = 21, Name =  "Adventure" } },
                { Genres.Veicoli, new  AnimeUnityGenre { Id = 29, Name = "Cars" } },
                { Genres.Commedia, new  AnimeUnityGenre { Id = 37, Name = "Comedy" } },
                { Genres.Demenziale, new  AnimeUnityGenre { Id = 43, Name = "Dementia" } },
                { Genres.Demoni, new  AnimeUnityGenre { Id = 13, Name = "Demons" } },
                { Genres.Drammatico, new  AnimeUnityGenre { Id = 22, Name = "Drama" } },
                { Genres.Ecchi, new  AnimeUnityGenre { Id = 5, Name = "Ecchi" } },
                { Genres.Fantasy, new  AnimeUnityGenre { Id = 9, Name = "Fantasy" } },
                { Genres.Gioco, new  AnimeUnityGenre { Id = 44, Name = "Game" } },
                { Genres.Harem, new  AnimeUnityGenre { Id = 15, Name = "Harem" } },
                { Genres.Hentai, new  AnimeUnityGenre { Id = 4, Name = "Hentai" } },
                { Genres.Storico, new  AnimeUnityGenre { Id = 30, Name = "Historical" } },
                { Genres.Horror, new  AnimeUnityGenre { Id = 3, Name = "Horror" } },
                { Genres.Josei, new  AnimeUnityGenre { Id = 45, Name = "Josei" } },
                { Genres.Bambini, new  AnimeUnityGenre { Id = 14, Name = "Kids" } },
                { Genres.Magia, new  AnimeUnityGenre { Id = 23, Name = "Magic" } },
                { Genres.ArtiMarziali, new  AnimeUnityGenre { Id = 31, Name = "Martial Arts" } },
                { Genres.Mecha, new  AnimeUnityGenre { Id = 38, Name = "Mecha" } },
                { Genres.Militari, new  AnimeUnityGenre { Id = 46, Name = "Military" } },
                { Genres.Mistero, new  AnimeUnityGenre { Id = 24, Name = "Mistery" } },
                { Genres.Musicale, new  AnimeUnityGenre { Id = 16, Name = "Music" } },
                { Genres.Parodia, new  AnimeUnityGenre { Id = 32, Name = "Parody" } },
                { Genres.Polizia, new  AnimeUnityGenre { Id = 39, Name = "Police" } },
                { Genres.Psicologico, new  AnimeUnityGenre { Id = 47, Name = "Psychological" } },
                { Genres.Romantico, new  AnimeUnityGenre { Id = 17, Name = "Romance" } },
                { Genres.Samurai, new  AnimeUnityGenre { Id = 25, Name = "Samurai" } },
                { Genres.Scolastico, new  AnimeUnityGenre { Id = 33, Name = "School" } },
                { Genres.SciFi, new  AnimeUnityGenre { Id = 40, Name = "Sci - fi" } },
                { Genres.Seinen, new  AnimeUnityGenre { Id = 49, Name = "Seinen" } },
                { Genres.Shoujo, new  AnimeUnityGenre { Id = 18, Name = "Shoujo" } },
                { Genres.ShoujoAi, new  AnimeUnityGenre { Id = 26, Name = "Shoujo Ai" } },
                { Genres.Shounen, new  AnimeUnityGenre { Id = 34, Name = "Shounen" } },
                { Genres.ShounenAi, new  AnimeUnityGenre { Id = 41, Name = "Shounen Ai" } },
                { Genres.SliceOfLife, new  AnimeUnityGenre { Id = 50, Name = "Slice of Life" } },
                { Genres.Spazio, new  AnimeUnityGenre { Id = 19, Name = "Space" } },
                { Genres.Sport, new  AnimeUnityGenre { Id = 27, Name = "Sports" } },
                { Genres.Superpoteri, new  AnimeUnityGenre { Id = 35, Name = "Super Power" } },
                { Genres.Soprannaturale, new  AnimeUnityGenre { Id = 42, Name = "Supernatural" } },
                { Genres.Thriller, new  AnimeUnityGenre { Id = 48, Name = "Thriller" } },
                { Genres.Vampiri, new  AnimeUnityGenre { Id = 20, Name = "Vampire" } },
                { Genres.Yaoi, new  AnimeUnityGenre { Id = 28, Name = "Yaoi" } },
                { Genres.Yuri, new  AnimeUnityGenre { Id = 36, Name = "Yuri" } },
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

        public string GetStatus(Statuses status)
        {
            Dictionary<Statuses, string> statusMap = new Dictionary<Statuses, string>()
            {
                { Statuses.InProgress, "In Corso"},
                { Statuses.Completed, "Terminato"},
                { Statuses.NotReleased, "In Uscita"},
                { Statuses.Canceled, "Droppato"},
            };

            return statusMap[status];
        }
    }
}
