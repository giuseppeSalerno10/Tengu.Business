using Flurl.Http;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Objects;
using Tengu.Business.Core.DTO.Output.AnimeSaturn;
using Tengu.Business.Core.Utilities.Interfaces;

namespace Tengu.Business.Core.Utilities
{
    public class AnimeSaturnUtilities : IAnimeSaturnUtilities
    {
        public async Task<AnimeSaturnCreateSessionOutput> CreateSession()
        {
            var requestUrl = Config.AnimeSaturnConfig.BaseUrl;

            var headResponse = await requestUrl
                .WithHeader("User-Agent", Config.HttpConfig.UserAgent)
                .HeadAsync();

            var sessionId = headResponse.Cookies.First(cookie => cookie.Name.Equals("PHPSESSID")).Value;


            var sessionResult = new AnimeSaturnCreateSessionOutput()
            {
                SessionId = sessionId.Replace("%3D", "="),
            };

            return sessionResult;
        }

        public string[] GetGenreArray(IEnumerable<Genres> genres)
        {
            Dictionary<Genres, string> genreMap = new Dictionary<Genres, string>()
            {
                { Genres.ArtiMarziali, "Arti Marziali" },
                { Genres.Avventura, "Avventura" },
                { Genres.Azione, "Azione" },
                { Genres.Bambini, "Bambini" },
                { Genres.Commedia, "Commedia" },
                { Genres.Demenziale, "Demenziale" },
                { Genres.Demoni, "Demoni" },
                { Genres.Drammatico, "Drammatico" },
                { Genres.Ecchi, "Ecchi" },
                { Genres.Fantasy, "Fantasy" },
                { Genres.Gioco, "Gioco" },
                { Genres.Harem, "Harem" },
                { Genres.Hentai, "Hentai" },
                { Genres.Horror, "Horror" },
                { Genres.Josei, "Josei" },
                { Genres.Magia, "Magia" },
                { Genres.Mecha, "Mecha" },
                { Genres.Militari, "Militari" },
                { Genres.Mistero, "Mistero" },
                { Genres.Musicale, "Musicale" },
                { Genres.Parodia, "Parodia" },
                { Genres.Polizia, "Polizia" },
                { Genres.Psicologico, "Psicologico" },
                { Genres.Romantico, "Romantico" },
                { Genres.Samurai, "Samurai" },
                { Genres.SciFi, "Sci-Fi" },
                { Genres.Scolastico, "Scolastico" },
                { Genres.Seinen, "Seinen" },
                { Genres.Sentimentale, "Sentimentale" },
                { Genres.ShoujoAi, "Shoujo Ai" },
                { Genres.Shoujo, "Shoujo" },
                { Genres.ShounenAi, "Shounen Ai" },
                { Genres.Shounen, "Shounen" },
                { Genres.SliceOfLife, "Slice of Life" },
                { Genres.Soprannaturale, "Soprannaturale" },
                { Genres.Spazio, "Spazio" },
                { Genres.Sport, "Sport" },
                { Genres.Storico, "Storico" },
                { Genres.Superpoteri, "Superpoteri" },
                { Genres.Thriller, "Thriller" },
                { Genres.Vampiri, "Vampiri" },
                { Genres.Veicoli, "Veicoli" },
                { Genres.Yaoi, "Yaoi" },
                { Genres.Yuri, "Yuri" }
            };

            var genresList = new List<string>();

            foreach (var genre in genres)
            {
                string? mappedGenre;
                genreMap.TryGetValue(genre, out mappedGenre);

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
                { Statuses.InProgress, "0"},
                { Statuses.Completed, "1"},
                { Statuses.NotReleased, "2"},
                { Statuses.Canceled, "3"},
            };

            return statusMap[status];
        }
    }
}
