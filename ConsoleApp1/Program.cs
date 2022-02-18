using Microsoft.Extensions.Hosting;
using Tengu.Business.API;
using Tengu.Business.Commons;

#region DI
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddTenguServices())
    .Build();

Task.Run( () => App(host.Services));

await host.RunAsync();
#endregion

async static void App(IServiceProvider services)
{
    async static Task TitleMenu(ITenguApi tenguApi)
    {
        Console.WriteLine("Inserisci il nome dell'anime");

        var title = Console.ReadLine() ?? "";

        var animes = await tenguApi.SearchAnime(title);

        Console.WriteLine("Anime:\n");

        for (int i = 0; i < animes.Length; i++)
        {
            AnimeModel? item = animes[i];
            Console.WriteLine($"[{i}] {item.Title}");
        }

        Console.WriteLine("\nScegli: ");

        var animeIndex = Convert.ToInt32(Console.ReadLine() ?? "0");

        for (int i = 0; i < animes[animeIndex].Episodes.Length; i++)
        {
            EpisodeModel? item = animes[animeIndex].Episodes[i];
            Console.WriteLine($"[{i}] {item.Title}");
        }

        Console.WriteLine("\nScegli per download (-1 per annullare): ");

        var episodeIndex = Convert.ToInt32(Console.ReadLine() ?? "-1");

        if (episodeIndex >= 0)
        {
            await tenguApi.Download(animes[animeIndex].Episodes[episodeIndex]);
        }
    }
    async static Task FilterMenu(ITenguApi tenguApi)
    {
        var searchFilter = new SearchFilter()
        {
            Genres = new Genres[] { Genres.ArtiMarziali },
            Years = new string[] { "2022" }
        };

        var animes = await tenguApi.SearchAnime(searchFilter);

        Console.WriteLine("Anime:\n");

        for (int i = 0; i < animes.Length; i++)
        {
            AnimeModel? item = animes[i];
            Console.WriteLine($"[{i}] {item.Title}");
        }

        Console.WriteLine("\nScegli: ");

        var animeIndex = Convert.ToInt32(Console.ReadLine() ?? "0");

        for (int i = 0; i < animes[animeIndex].Episodes.Length; i++)
        {
            EpisodeModel? item = animes[animeIndex].Episodes[i];
            Console.WriteLine($"[{i}] {item.Title}");
        }

        Console.WriteLine("\nScegli per download (-1 per annullare): ");

        var episodeIndex = Convert.ToInt32(Console.ReadLine() ?? "-1");

        if (episodeIndex >= 0)
        {
            await tenguApi.Download(animes[animeIndex].Episodes[episodeIndex]);
        }
    }
    async static Task BothMenu(ITenguApi tenguApi)
    {
        Console.WriteLine("Inserisci il nome dell'anime");

        var title = Console.ReadLine() ?? "";

        var searchFilter = new SearchFilter()
        {
            Genres = new Genres[] { Genres.ArtiMarziali },
            Years = new string[] { "2022" }
        };

        var animes = await tenguApi.SearchAnime(title, searchFilter);

        Console.WriteLine("Anime:\n");

        for (int i = 0; i < animes.Length; i++)
        {
            AnimeModel? item = animes[i];
            Console.WriteLine($"[{i}] {item.Title}");
        }

        Console.WriteLine("\nScegli: ");

        var animeIndex = Convert.ToInt32(Console.ReadLine() ?? "0");

        for (int i = 0; i < animes[animeIndex].Episodes.Length; i++)
        {
            EpisodeModel? item = animes[animeIndex].Episodes[i];
            Console.WriteLine($"[{i}] {item.Title}");
        }

        Console.WriteLine("\nScegli per download (-1 per annullare): ");

        var episodeIndex = Convert.ToInt32(Console.ReadLine() ?? "-1");

        if (episodeIndex >= 0)
        {
            await tenguApi.Download(animes[animeIndex].Episodes[episodeIndex]);
        }
    }

    ITenguApi tenguApi = (ITenguApi) (services.GetService(typeof(ITenguApi)) ?? throw new Exception());

    tenguApi.CurrentHosts = new Hosts[] { Hosts.AnimeSaturn };

    Console.WriteLine("\n#####################\n" +
        "Inserisci il tipo di ricerca:" +
        "\n0 - Title" +
        "\n1 - Filter" +
        "\n2 - Mista");

    switch (Convert.ToInt32(Console.ReadLine()))
    {
        case 0:
            await TitleMenu(tenguApi);
            break;
        case 1:
            await FilterMenu(tenguApi);
            break;
        case 2:
            await BothMenu(tenguApi);
            break;
            
        default:
            return;
    }
    
}