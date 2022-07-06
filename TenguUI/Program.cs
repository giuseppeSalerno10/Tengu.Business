using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tengu.Business.API;
using TenguUI.Controllers;
using TenguUI.Controllers.Interfaces;
using TenguUI.Managers;
using TenguUI.Managers.Interfaces;

namespace TenguUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var host = CreateHostBuilder().Build();

            Application.Run(host.Services.GetRequiredService<App>());
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services.AddTenguServices();

                    services.AddSingleton<App>();

                    services.AddSingleton<ITenguController, TenguController>();
                    services.AddSingleton<ITenguManager, TenguManager>();

                    services.AddLogging();
                });
        }
    }
}