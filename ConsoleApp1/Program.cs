using Tengu.Business.Commons;
using Tengu.Business.Core;

string downloadPath = "C:\\Users\\Giuse\\Desktop";

Dictionary<Genres, string> genreMap = new Dictionary<Genres, string>()
            {
                { Genres.Martial, "Arti Marziali" },
                { Genres.Adventure, "Avventura" }
            };
Dictionary<Statuses, string> statusMap = new Dictionary<Statuses, string>()
            {
                { Statuses.InProgress, "0"},
                { Statuses.Completed, "1"},
                { Statuses.NotReleased, "2"},
                { Statuses.Canceled, "3"},
            };
Dictionary<Languages, string> langMap = new Dictionary<Languages, string>()
            {
                { Languages.Dubbed, "1" },
                { Languages.Subbed , "0" }
            };

AnimeSaturnAdapters animeSaturnAdapters = new AnimeSaturnAdapters();

var searchType = "FILTER"; // TITLE | FILTER

var animes = searchType switch
{
    "TITLE" => await animeSaturnAdapters.SearchByTitleAsync("Atta"),
    "FILTER" => await animeSaturnAdapters.SearchByFiltersAsync(new AnimeSaturnSearchFilterInput()
            {
                Genres = new string[] { genreMap[Genres.Martial] },
                Language = langMap[Languages.Dubbed],
                Statuses = new string[] { statusMap[Statuses.Completed] },
                Years = new string[] { "2021" }
            }),
    _ => null
};

var episodes = await animeSaturnAdapters.GetLatestEpisodeAsync(30);


while (animes != null)
{
    for (int i = 0; i < animes.Length; i++)
    {
        var anime = animes[i];
        Console.Write($"[{i}] {anime.Title}: ");
        
        for (int j = 0; j < anime.Episodes.Length; j++)
        {
            var episode = anime.Episodes[j];
            Console.Write($"E{j}, ");
        }
        Console.WriteLine();
    }
    Console.WriteLine("Inserisci anime (0-N)");
    var animeIndex = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("Inserisci Episodio (0-N)");
    var episodeIndex = Convert.ToInt32(Console.ReadLine());

    try
    {
        await animeSaturnAdapters.Download(downloadPath, animes[animeIndex].Episodes[episodeIndex].Url);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message + "\nURL: " + animes[animeIndex].Episodes[episodeIndex].Url);
    }

    Console.Clear();
}

while (episodes != null)
{
    for (int i = 0; i < episodes.Length; i++)
    {
        var episode = episodes[i];
        Console.WriteLine($"[{i}] {episode.Title}: ");
    }

    Console.WriteLine("Inserisci Episodio (0-N)");
    var episodeIndex = Convert.ToInt32(Console.ReadLine());

    await animeSaturnAdapters.Download(downloadPath, episodes[episodeIndex].Url);

}
