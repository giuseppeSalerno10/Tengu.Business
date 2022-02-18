using Microsoft.Extensions.Hosting;
using Tengu.Business.API;

#region DI
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddTenguServices())
    .Build();


App(host.Services);

await host.RunAsync();
#endregion

static void App( IServiceProvider services)
{

    string downloadPath = "C:\\Users\\Giuse\\Desktop";

    var tenguApi = services.GetService(typeof(TenguApi));


}




