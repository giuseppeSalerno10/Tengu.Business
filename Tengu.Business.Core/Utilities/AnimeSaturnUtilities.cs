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

        public string[] GetGenreArray(IEnumerable<TenguGenres> genres)
        {
            Dictionary<TenguGenres, string> genreMap = new Dictionary<TenguGenres, string>()
            {
                { TenguGenres.ArtiMarziali, "Arti Marziali" },
                { TenguGenres.Avventura, "Avventura" },
                { TenguGenres.Azione, "Azione" },
                { TenguGenres.Bambini, "Bambini" },
                { TenguGenres.Commedia, "Commedia" },
                { TenguGenres.Demenziale, "Demenziale" },
                { TenguGenres.Demoni, "Demoni" },
                { TenguGenres.Drammatico, "Drammatico" },
                { TenguGenres.Ecchi, "Ecchi" },
                { TenguGenres.Fantasy, "Fantasy" },
                { TenguGenres.Gioco, "Gioco" },
                { TenguGenres.Harem, "Harem" },
                { TenguGenres.Hentai, "Hentai" },
                { TenguGenres.Horror, "Horror" },
                { TenguGenres.Josei, "Josei" },
                { TenguGenres.Magia, "Magia" },
                { TenguGenres.Mecha, "Mecha" },
                { TenguGenres.Militari, "Militari" },
                { TenguGenres.Mistero, "Mistero" },
                { TenguGenres.Musicale, "Musicale" },
                { TenguGenres.Parodia, "Parodia" },
                { TenguGenres.Polizia, "Polizia" },
                { TenguGenres.Psicologico, "Psicologico" },
                { TenguGenres.Romantico, "Romantico" },
                { TenguGenres.Samurai, "Samurai" },
                { TenguGenres.SciFi, "Sci-Fi" },
                { TenguGenres.Scolastico, "Scolastico" },
                { TenguGenres.Seinen, "Seinen" },
                { TenguGenres.Sentimentale, "Sentimentale" },
                { TenguGenres.ShoujoAi, "Shoujo Ai" },
                { TenguGenres.Shoujo, "Shoujo" },
                { TenguGenres.ShounenAi, "Shounen Ai" },
                { TenguGenres.Shounen, "Shounen" },
                { TenguGenres.SliceOfLife, "Slice of Life" },
                { TenguGenres.Soprannaturale, "Soprannaturale" },
                { TenguGenres.Spazio, "Spazio" },
                { TenguGenres.Sport, "Sport" },
                { TenguGenres.Storico, "Storico" },
                { TenguGenres.Superpoteri, "Superpoteri" },
                { TenguGenres.Thriller, "Thriller" },
                { TenguGenres.Vampiri, "Vampiri" },
                { TenguGenres.Veicoli, "Veicoli" },
                { TenguGenres.Yaoi, "Yaoi" },
                { TenguGenres.Yuri, "Yuri" }
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
        public string GetStatus(TenguStatuses status)
        {
            Dictionary<TenguStatuses, string> statusMap = new Dictionary<TenguStatuses, string>()
            {
                { TenguStatuses.InProgress, "0"},
                { TenguStatuses.Completed, "1"},
                { TenguStatuses.NotReleased, "2"},
                { TenguStatuses.Canceled, "3"},
            };

            return statusMap[status];
        }
    }
}
