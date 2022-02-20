using Microsoft.Extensions.Hosting;
using Tengu.Business.API;
using Tengu.Business.Commons;

#region DI
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddTenguServices())
    .Build();

host.Start();

await App(host.Services);

#endregion

async static Task App(IServiceProvider services)
{

    AnimeModel[] currentAnimes = Array.Empty<AnimeModel>();
    EpisodeModel[] currentEpisodes = Array.Empty<EpisodeModel>();

    ITenguApi tenguApi = (ITenguApi) (services.GetService(typeof(ITenguApi)) ?? throw new Exception());

    tenguApi.CurrentHosts = new Hosts[] { Hosts.AnimeSaturn };

    while (true)
    {
        Console.Clear();

        Console.WriteLine(
            "Inserisci il tipo di operazione:" +
            "\n0 - Ricerca Anime" +
            "\n1 - Fetch Episodi" +
            "\n2 - Download Episodio" +
            "\n3 - Kitsu"
            );

        switch (Convert.ToInt32(Console.ReadLine()))
        {
            case 0:
                Console.Clear();
                currentAnimes = await SearchAnimeMenu(tenguApi);
                break;
            case 1:
                Console.Clear();
                currentEpisodes = await GetEpisodesMenu(tenguApi, currentAnimes);
                break;
            case 2:
                Console.Clear();
                await DownloadEpisodeMenu(tenguApi, currentEpisodes);
                break;
            case 3:
                Console.Clear();
                await KitsuMenu(tenguApi);
                break;

            default:
                return;
        }

        Console.WriteLine("\nPremi un pulsante per continuare");
        Console.ReadKey();
    }
}


async static Task<AnimeModel[]> SearchAnimeMenu(ITenguApi tenguApi)
{
    Console.WriteLine(
        "Scegli il tipo di ricerca:\n" +
        "0 - Titolo\n" +
        "1 - Filtri\n" +
        "2 - Filtri e Titolo"
        );

    AnimeModel[] animes = Array.Empty<AnimeModel>();

    switch (Convert.ToInt32(Console.ReadLine()))
    {
        case 0:
            Console.Clear();
            Console.WriteLine("Inserisci il titolo");

            var title = Console.ReadLine();

            if(title != null)
            {
                Console.WriteLine("\nRisultati:");
                animes = await tenguApi.SearchAnime(title);
                for (int i = 0; i < animes.Length; i++)
                {
                    AnimeModel? anime = animes[i];
                    Console.WriteLine($"[{i}] {anime.Title} - {anime.Id}");
                }
            }

            break;

        case 1:
            Console.Clear();

            var filter = new SearchFilter() 
            { 
                Genres = new Genres[] { Genres.ArtiMarziali }
            };

            Console.WriteLine("Risultati:");
            animes = await tenguApi.SearchAnime(filter);

            for (int i = 0; i < animes.Length; i++)
            {
                AnimeModel? anime = animes[i];
                Console.WriteLine($"[{i}] {anime.Title} - {anime.Id}");
            }
            break;

        case 2:
            Console.Clear();

            Console.WriteLine("Inserisci il titolo");
            var titleWithFilter = Console.ReadLine() ?? "";

            var filterWithTitle = new SearchFilter()
            {
                Genres = new Genres[] { Genres.ArtiMarziali }
            };

            Console.WriteLine("Risultati:");
            animes = await tenguApi.SearchAnime(titleWithFilter, filterWithTitle);

            for (int i = 0; i < animes.Length; i++)
            {
                AnimeModel? anime = animes[i];
                Console.WriteLine($"[{i}] {anime.Title} - {anime.Id}");
            }
            break;
        default:
            Console.WriteLine("Input Errato");
            break;
    }

    return animes;
}

async static Task<EpisodeModel[]> GetEpisodesMenu(ITenguApi tenguApi, AnimeModel[] animes)
{
    Console.WriteLine(
    "Lista Anime:\n"
    );

    EpisodeModel[] episodes = new EpisodeModel[0];

    for (int i = 0; i < animes.Length; i++)
    {
        AnimeModel? anime = animes[i];
        Console.WriteLine($"[{i}] {anime.Title} - {anime.Id}");
    }
    Console.WriteLine(
        "\nScegli un anime [0-n]:"
        );

    var animeIndex = Convert.ToInt32(Console.ReadLine() ?? throw new Exception(""));

    episodes = await tenguApi.GetEpisodes(animes[animeIndex].Id, animes[animeIndex].Host, 1, 2);

    Console.WriteLine($"Lista episodi ({episodes.Length}):");
    for (int i = 0; i < episodes.Length; i++)
    {
        EpisodeModel? epsisode = episodes[i];
        Console.WriteLine($"[{i}] {epsisode.Title} - {epsisode.Id}");
    }

    return episodes;
}

static Task DownloadEpisodeMenu(ITenguApi tenguApi, EpisodeModel[] episodes)
{
    throw new NotImplementedException();
}

static Task KitsuMenu(ITenguApi tenguApi)
{
    throw new NotImplementedException();
}