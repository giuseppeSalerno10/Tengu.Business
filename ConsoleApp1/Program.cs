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
    async static Task TitleMenu(ITenguApi tenguApi)
    {
        Console.WriteLine("Inserisci il nome dell'anime");

        var title = Console.ReadLine() ?? "";

        var animes = await tenguApi.SearchAnime(title, true);

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

        Console.WriteLine("\nScegli per download (invio per annullare): ");


        var rawIndexes = Console.ReadLine();
        var episodeIndexes = rawIndexes.Split(",");

        if (episodeIndexes.Length > 0)
        {
            foreach (var index in episodeIndexes)
            {
                await tenguApi.Download(animes[animeIndex].Episodes[Convert.ToInt32(index)]);
            }
        }

        Console.WriteLine("Adieu!");
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
    async static Task KitsuMenu(ITenguApi tenguApi)
    {

        var animes = await tenguApi.KitsuUpcomingAnime(10);

        Console.WriteLine("Anime:\n");

        for (int i = 0; i < animes.Length; i++)
        {
            AnimeModel? item = animes[i];
            Console.WriteLine($"[{i}] {item.Title}");
        }

    }
    ITenguApi tenguApi = (ITenguApi) (services.GetService(typeof(ITenguApi)) ?? throw new Exception());

    tenguApi.CurrentHosts = new Hosts[] { Hosts.AnimeSaturn };

    while (true)
    {
        Console.Clear();

        Console.WriteLine("#####################\n" +
            "Inserisci il tipo di ricerca:" +
            "\n0 - Title" +
            "\n1 - Filter" +
            "\n2 - Mista" +
            "\n3 - Kitsu");

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
            case 3:
                await KitsuMenu(tenguApi);
                break;

            default:
                return;
        }
    }
}