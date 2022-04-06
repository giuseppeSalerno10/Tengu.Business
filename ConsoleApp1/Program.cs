using Downla;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Collections.Concurrent;
using Tengu.Business.API;
using Tengu.Business.Commons;

#region DI
using IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog(
    (hostingContext, loggerConfiguration) => loggerConfiguration.WriteTo.File($"C:\\Users\\Giuse\\Desktop\\log.txt"))
    .ConfigureServices((_, services) => {
        services.AddTenguServices();
    })
    .Build();

var tengu = (TenguApi) ActivatorUtilities.CreateInstance(host.Services, typeof(TenguApi));

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
            "\n1 - Ultimi Episodi" +
            "\n2 - Fetch Episodi" +
            "\n3 - Download Episodio" +
            "\n4 - Kitsu" +
            "\n5 - Calendario" +
            "\n6 - Cambia Host"
            );

        switch (Convert.ToInt32(Console.ReadLine()))
        {
            case 0:
                Console.Clear();
                currentAnimes = await SearchAnimeMenu(tenguApi);
                break;
            case 1:
                Console.Clear();
                currentEpisodes = await GetLatestEpisodesMenu(tenguApi);
                break;
            case 2:
                Console.Clear();
                currentEpisodes = await GetEpisodesMenu(tenguApi, currentAnimes);
                break;
            case 3:
                Console.Clear();
                DownloadEpisodeMenu(tenguApi, currentEpisodes);
                break;
            case 4:
                Console.Clear();
                await KitsuMenu(tenguApi);
                break;
            case 5:
                Console.Clear();
                await GetCalendarMenu(tenguApi);
                break;
            case 6:
                Console.Clear();
                ChangeHostMenu(tenguApi);
                break;
            default:
                return;
        }

        Console.WriteLine("\nPremi un pulsante per continuare");
        Console.ReadKey();
    }
}

static void ChangeHostMenu(ITenguApi tenguApi)
{
    Console.WriteLine("Host Correnti");
    foreach (var host in tenguApi.CurrentHosts)
    {
        Console.WriteLine($"{host}");
    }

    Console.WriteLine("Scegli gli hosts:" +
            "\n0 - Entrambi" +
            "\n1 - AnimeUnity" +
            "\n2 - AnimeSaturn"
            );
    switch (Convert.ToInt32(Console.ReadLine()))
    {
        case 0:
            Console.Clear();
            tenguApi.CurrentHosts = new Hosts[] { Hosts.AnimeSaturn, Hosts.AnimeUnity };
            break;
        case 1:
            Console.Clear();
            tenguApi.CurrentHosts = new Hosts[] {  Hosts.AnimeUnity };
            break;
        case 2:
            Console.Clear();
            tenguApi.CurrentHosts = new Hosts[] { Hosts.AnimeSaturn };
            break;
        default:
            return;
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
                animes = await tenguApi.SearchAnimeAsync(title);
                for (int i = 0; i < animes.Length; i++)
                {
                    AnimeModel? anime = animes[i];
                    Console.WriteLine($"[{i}] {anime.Title} - {anime.Id} - {anime.Host}");
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
            animes = await tenguApi.SearchAnimeAsync(filter);

            for (int i = 0; i < animes.Length; i++)
            {
                AnimeModel? anime = animes[i];
                Console.WriteLine($"[{i}] {anime.Title} - {anime.Id} - {anime.Host}");
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
            animes = await tenguApi.SearchAnimeAsync(titleWithFilter, filterWithTitle);

            for (int i = 0; i < animes.Length; i++)
            {
                AnimeModel? anime = animes[i];
                Console.WriteLine($"[{i}] {anime.Title} - {anime.Id} - {anime.Host}");
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
        Console.WriteLine($"[{i}] {anime.Title} - {anime.Id} - {anime.Host}");
    }
    Console.WriteLine(
        "\nScegli un anime [0-n]:"
        );

    var animeIndex = Convert.ToInt32(Console.ReadLine() ?? throw new Exception(""));

    episodes = await tenguApi.GetEpisodesAsync(animes[animeIndex].Id, animes[animeIndex].Host);

    Console.WriteLine($"Lista episodi ({episodes.Length}):");
    for (int i = 0; i < episodes.Length; i++)
    {
        EpisodeModel? epsisode = episodes[i];
        Console.WriteLine($"[{i}] {epsisode.Title} - {epsisode.Id} - {epsisode.Host}");
    }

    return episodes;
}
async static Task<EpisodeModel[]> GetLatestEpisodesMenu(ITenguApi tenguApi)
{
    Console.WriteLine("Inserisci lowe e uppe");
    var lowe = Convert.ToInt32(Console.ReadLine());
    var uppe = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine(
    "Lista Episodi:\n"
    );

    var episodes = await tenguApi.GetLatestEpisodeAsync(lowe,uppe);

    Console.WriteLine($"Lista episodi ({episodes.Length}):");
    for (int i = 0; i < episodes.Length; i++)
    {
        EpisodeModel? epsisode = episodes[i];
        Console.WriteLine($"[{i}] {epsisode.Title} - {epsisode.Id} - {epsisode.Host}");
    }

    return episodes;
}
static void DownloadEpisodeMenu(ITenguApi tenguApi, EpisodeModel[] episodes)
{
    Console.WriteLine("Lista Episodi");

    for (int i = 0; i < episodes.Length; i++)
    {
        EpisodeModel? ep = episodes[i];
        Console.WriteLine($"[{i}] {ep.Title} - {ep.Id}");
    }
    Console.WriteLine(
        "\nScegli gli episodi da scaricare (divisore ,) [0-n]:"
        );

    var episodeIndexes = (Console.ReadLine() ?? throw new Exception("")).Split(",");

    var queues = new Dictionary<Hosts, List<EpisodeModel>>();

    foreach(var host in tenguApi.CurrentHosts)
    {
        queues.Add(host, new List<EpisodeModel>());
    }

    foreach (var episodeIndex in episodeIndexes)
    {
        var episode = episodes[Convert.ToInt32(episodeIndex)];
        queues[episode.Host].Add(episode);
    }

    var tasks = new List<Task>();

    var downloadList = new ConcurrentBag<DownloadInfosModel>();

    foreach (var queue in queues)
    {
        tasks.Add(
            Task.Run( async () => 
            {
                foreach (var episode in queue.Value)
                {
                    var download = tenguApi.DownloadAsync(episode.DownloadUrl, episode.Host);

                    downloadList.Add(download);

                    await download.EnsureDownloadCompletation();
                }
            })
        );
    }

    while (tasks.Count > 0)
    {
        Console.Clear();
        foreach (var download in downloadList)
        {
            if (download.TotalPackets != 0)
            {
                Console.WriteLine($"{download.FileName} Percentage: {download.DownloadedPackets * 100 / download.TotalPackets} %");
            }
        }

        foreach (var task in tasks.ToArray())
        {
            if (task.IsCompleted)
            {
                tasks.Remove(task);
            }
        }
        Thread.Sleep(5000);
    }

    Console.WriteLine("Anime scaricati");

}
async static Task<KitsuAnimeModel[]> KitsuMenu(ITenguApi tenguApi)
{
    Console.WriteLine(
        "Scegli il tipo di operazione:\n" +
        "0 - Upcoming\n" +
        "1 - Ricerca per titolo\n"
        );

    KitsuAnimeModel[] animes = Array.Empty<KitsuAnimeModel>();

    switch (Convert.ToInt32(Console.ReadLine()))
    {
        case 0:
            Console.Clear();

            Console.WriteLine("\nRisultati:");
            animes = await tenguApi.KitsuUpcomingAnimeAsync(0, 25);
            for (int i = 0; i < animes.Length; i++)
            {
                KitsuAnimeModel? anime = animes[i];
                Console.WriteLine($"[{i}] {anime.Title}");
            }


            break;

        case 1:
            Console.Clear();

            Console.Clear();
            Console.WriteLine("Inserisci il titolo");

            var title = Console.ReadLine();
            
            if(title != null)
            {
                Console.WriteLine("\nRisultati:");
                animes = await tenguApi.KitsuSearchAnimeAsync(title, 0, 3);
                for (int i = 0; i < animes.Length; i++)
                {
                    KitsuAnimeModel? anime = animes[i];
                    Console.WriteLine($"[{i}] {anime.Title}");
                }
            }

            break;

        default:
            Console.WriteLine("Input Errato");
            break;
    }

    return animes;
}
async static Task GetCalendarMenu(ITenguApi tenguApi)
{
    Console.WriteLine("Calendario");

    var calendar = await tenguApi.GetCalendar();

}
