using Downla;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API.Controller;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.Commons;
using Tengu.Business.Core;

namespace Tengu.Business.API
{
    public static class TenguServicesExtensions
    {
        public static IServiceCollection AddTenguServices(this IServiceCollection services)
        {
            services.AddSingleton<ITenguApi, TenguApi>();

            services.AddTransient<IAnimeSaturnController, AnimeSaturnController>();
            services.AddTransient<IAnimeUnityController, AnimeUnityController>();
            services.AddTransient<IKitsuController, KitsuController>();

            services.AddTransient<IAnimeSaturnManager, AnimeSaturnManager>();
            services.AddTransient<IAnimeUnityManager, AnimeUnityManager>();
            services.AddTransient<IKitsuManager, KitsuManager>();

            services.AddTransient<IAnimeSaturnAdapter, AnimeSaturnAdapter>();
            services.AddTransient<IAnimeUnityAdapter, AnimeUnityAdapter>();
            services.AddTransient<IKitsuAdapter, KitsuAdapter>();

            services.AddSingleton<IAnimeSaturnUtilities, AnimeSaturnUtilities>();
            services.AddSingleton<IAnimeUnityUtilities, AnimeUnityUtilities>();

            services.AddDownlaServices();
            return services;
        }
    }
}
