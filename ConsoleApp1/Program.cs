using Downla;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Collections.Concurrent;
using Tengu.Business.API;
using Tengu.Business.API.DTO;
using Tengu.Business.API.Interfaces;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;

#region DI
using IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog(
    (hostingContext, loggerConfiguration) => loggerConfiguration.WriteTo.File($"C:\\Users\\Giuse\\Desktop\\log.txt"))
    .ConfigureServices((_, services) =>
    {
        services.AddTenguServices();
    })
    .Build();

var tengu = (TenguApi)ActivatorUtilities.CreateInstance(host.Services, typeof(TenguApi));

host.Start();

await App(host.Services);
#endregion

async static Task App(IServiceProvider services)
{

    AnimeModel[] currentAnimes = Array.Empty<AnimeModel>();
    EpisodeModel[] currentEpisodes = Array.Empty<EpisodeModel>();

    ITenguApi tenguApi = (ITenguApi)(services.GetService(typeof(ITenguApi)) ?? throw new Exception());

    tenguApi.CurrentHosts = new Hosts[] { Hosts.AnimeSaturn };

    while (true)
    {
        Console.Clear();

        Console.WriteLine(
            "Inserisci il tipo di operazione:" +
            "\n0 - Ricerca Anime" +
            "\n1 - Fetch Episodi" +
            "\n2 - Ultimi Episodi" +
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
                currentEpisodes = await GetEpisodesMenu(tenguApi, currentAnimes);
                break;
            case 2:
                Console.Clear();
                currentEpisodes = await GetLatestEpisodesMenu(tenguApi);
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
            tenguApi.CurrentHosts = new Hosts[] { Hosts.AnimeUnity };
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

    TenguResult<AnimeModel[]>[] results = null!;

    switch (Convert.ToInt32(Console.ReadLine()))
    {
        case 0:
            Console.Clear();
            Console.WriteLine("Inserisci il titolo");

            var title = Console.ReadLine();

            if (title != null)
            {
                Console.WriteLine("\nRisultati:");
                results = await tenguApi.SearchAnimeAsync(title);
                foreach (var result in results)
                {
                    if (result.Success)
                    {
                        for (int i = 0; i < result.Data.Length; i++)
                        {
                            AnimeModel? anime = result.Data[i];
                            Console.WriteLine($"[{i}] {anime.Title} - {anime.Id} - {anime.Host}");
                        }
                    }
                    else
                    {
                        Console.WriteLine(result.Exception!.Message);

                    }
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
            results = await tenguApi.SearchAnimeAsync(filter);
            foreach (var result in results)
            {
                if (result.Success)
                {
                    for (int i = 0; i < result.Data.Length; i++)
                    {
                        AnimeModel? anime = result.Data[i];
                        Console.WriteLine($"[{i}] {anime.Title} - {anime.Id} - {anime.Host}");
                    }
                }
                else
                {
                    Console.WriteLine(result.Exception!.Message);

                }
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
            results = await tenguApi.SearchAnimeAsync(titleWithFilter, filterWithTitle);

            foreach (var result in results)
            {
                if (result.Success)
                {
                    for (int i = 0; i < result.Data.Length; i++)
                    {
                        AnimeModel? anime = result.Data[i];
                        Console.WriteLine($"[{i}] {anime.Title} - {anime.Id} - {anime.Host}");
                    }
                }
                else
                {
                    Console.WriteLine(result.Exception!.Message);

                }
            }
            break;
        default:
            Console.WriteLine("Input Errato");
            break;
    }

    return results[0].Data;
}
async static Task<EpisodeModel[]> GetEpisodesMenu(ITenguApi tenguApi, AnimeModel[] animes)
{
    Console.WriteLine(
    "Lista Anime:\n"
    );

    for (int i = 0; i < animes.Length; i++)
    {
        AnimeModel? anime = animes[i];
        Console.WriteLine($"[{i}] {anime.Title} - {anime.Id} - {anime.Host}");
    }
    Console.WriteLine(
        "\nScegli un anime [0-n]:"
        );

    var animeIndex = Convert.ToInt32(Console.ReadLine() ?? throw new Exception(""));

    var results = await tenguApi.GetEpisodesAsync(animes[animeIndex].Id, animes[animeIndex].Host);

    if (results.Success)
    {
        Console.WriteLine($"Lista episodi ({results.Data.Length}):");
        for (int i = 0; i < results.Data.Length; i++)
        {
            EpisodeModel? epsisode = results.Data[i];
            Console.WriteLine($"[{i}] {epsisode.Title} - {epsisode.Id} - {epsisode.Host}");
        }
    }

    return results.Data;
}
async static Task<EpisodeModel[]> GetLatestEpisodesMenu(ITenguApi tenguApi)
{
    Console.WriteLine("Inserisci lower e upper");
    var lowe = Convert.ToInt32(Console.ReadLine());
    var uppe = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine(
    "Lista Episodi:\n"
    );

    var results = await tenguApi.GetLatestEpisodeAsync(lowe, uppe);
    foreach (var result in results)
    {
        if (result.Success)
        {
            Console.WriteLine($"Lista episodi {result.Host} ({result.Data.Length}):");
            for (int i = 0; i < result.Data.Length; i++)
            {
                EpisodeModel? epsisode = result.Data[i];
                Console.WriteLine($"[{i}] {epsisode.Title} - {epsisode.Id} - {epsisode.Host}");
            }
        }
        else
        {
            Console.WriteLine(result.Exception!.Message);
        }
    }
    return results[0].Data;
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

    foreach (var host in tenguApi.CurrentHosts)
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
            Task.Run(async () =>
            {
                foreach (var episode in queue.Value)
                {
                    var download = tenguApi.DownloadAsync(episode.DownloadUrl, episode.Host);
                    if (download.Success)
                    {
                        downloadList.Add(download.Data);

                        await download.Data.EnsureDownloadCompletation();
                    }

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

    TenguResult<KitsuAnimeModel[]> result = null!;

    switch (Convert.ToInt32(Console.ReadLine()))
    {
        case 0:
            Console.Clear();

            Console.WriteLine("\nRisultati:");

            result = await tenguApi.KitsuUpcomingAnimeAsync(0, 25);
            if (result.Success)
            {
                for (int i = 0; i < result.Data.Length; i++)
                {
                    KitsuAnimeModel? anime = result.Data[i];
                    Console.WriteLine($"[{i}] {anime.Title}");
                }
            }
            else
            {
                Console.WriteLine(result.Exception!.Message);
            }



            break;

        case 1:
            Console.Clear();

            Console.Clear();
            Console.WriteLine("Inserisci il titolo");

            var title = Console.ReadLine();

            if (title != null)
            {
                Console.WriteLine("\nRisultati:");

                result = await tenguApi.KitsuSearchAnimeAsync(title, 0, 3);
                if (result.Success)
                {
                    for (int i = 0; i < result.Data.Length; i++)
                    {
                        KitsuAnimeModel? anime = result.Data[i];
                        Console.WriteLine($"[{i}] {anime.Title}");
                    }
                }
                else
                {
                    Console.WriteLine(result.Exception!.Message);
                }

            }

            break;

        default:
            Console.WriteLine("Input Errato");
            break;
    }

    return result.Data;
}
async static Task GetCalendarMenu(ITenguApi tenguApi)
{
    Console.WriteLine("Calendario");

    var calendar = await tenguApi.GetCalendar();

}
