using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tengu.Business.API;
using TenguUI.Controllers;
using TenguUI.Controllers.Interfaces;
using TenguUI.Managers;
using TenguUI.Managers.Interfaces;
using Serilog;
using Serilog.Sinks.File;

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
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.WriteTo.File($"{Environment.CurrentDirectory}/log.txt"))
                .ConfigureServices((context, services) => {
                    services.AddTenguServices();

                    services.AddSingleton<App>();

                    services.AddSingleton<ITenguController, TenguController>();
                    services.AddSingleton<ITenguManager, TenguManager>();
                });
        }
    }
}