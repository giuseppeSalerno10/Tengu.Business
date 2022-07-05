using Downla;
using Downla.Models;
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
#endregion
