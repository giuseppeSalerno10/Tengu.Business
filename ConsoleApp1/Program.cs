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
                await SearchAnimeMenu(tenguApi);
                break;
            case 1:
                await GetEpisodesMenu(tenguApi);
                break;
            case 2:
                await DownloadEpisodeMenu(tenguApi);
                break;
            case 3:
                await KitsuMenu(tenguApi);
                break;

            default:
                return;
        }
    }
}


static Task SearchAnimeMenu(ITenguApi tenguApi)
{
    throw new NotImplementedException();
}

static Task GetEpisodesMenu(ITenguApi tenguApi)
{
    throw new NotImplementedException();
}

static Task DownloadEpisodeMenu(ITenguApi tenguApi)
{
    throw new NotImplementedException();
}

static Task KitsuMenu(ITenguApi tenguApi)
{
    throw new NotImplementedException();
}