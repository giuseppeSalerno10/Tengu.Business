using Tengu.Business.Commons;
using Tengu.Business.Core;

Dictionary<Genres, string> genreMap = new Dictionary<Genres, string>()
            {
                { Genres.Martial, "Arti Marziali" },
                { Genres.Adventure, "Avventura" }
            };
Dictionary<Statuses, string> statusMap = new Dictionary<Statuses, string>()
            {
                { Statuses.InProgress, "0"},
                { Statuses.NotReleased, "1"},
                { Statuses.Completed, "2"},
                { Statuses.Canceled, "3"},
            };
Dictionary<Languages, string> langMap = new Dictionary<Languages, string>()
            {
                { Languages.Dubbed, "1" },
                { Languages.Subbed , "0" }
            };

AnimeSaturnAdapters animeSaturnAdapters = new AnimeSaturnAdapters();
//var searchByTitleResult = animeSaturnAdapters.SearchByTitle("Atta");


var searchByFilterResult = animeSaturnAdapters.SearchByFilters(
    new AnimeSaturnSearchFilterInput()
    {
        Genres = new string[] { genreMap[Genres.Martial] },
        Language = langMap[Languages.Dubbed],
        Statuses = new string[] { statusMap[Statuses.NotReleased] },
        Years = new string[] { "2022", "2021", "2020" }
    }
);
