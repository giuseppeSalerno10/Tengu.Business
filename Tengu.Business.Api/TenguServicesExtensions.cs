using Downla;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API.Controller;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.API.Interfaces;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Services;
using Tengu.Business.Commons.Services.Interfaces;
using Tengu.Business.Core;

namespace Tengu.Business.API
{
    public static class TenguServicesExtensions
    {
        public static IServiceCollection AddTenguServices(this IServiceCollection services)
        {
            services.AddSingleton<ITenguApi, TenguApi>();

            services.AddSingleton<IManipulationService, ManipulationService>();

            services.AddSingleton<IAnimeSaturnController, AnimeSaturnController>();
            services.AddSingleton<IAnimeUnityController, AnimeUnityController>();
            services.AddSingleton<IKitsuController, KitsuController>();

            services.AddSingleton<IAnimeSaturnManager, AnimeSaturnManager>();
            services.AddSingleton<IAnimeUnityManager, AnimeUnityManager>();
            services.AddSingleton<IKitsuManager, KitsuManager>();

            services.AddSingleton<IAnimeSaturnAdapter, AnimeSaturnAdapter>();
            services.AddSingleton<IAnimeUnityAdapter, AnimeUnityAdapter>();
            services.AddSingleton<IKitsuAdapter, KitsuAdapter>();

            services.AddSingleton<IAnimeSaturnUtilities, AnimeSaturnUtilities>();
            services.AddSingleton<IAnimeUnityUtilities, AnimeUnityUtilities>();

            services.AddDownlaServices();
            return services;
        }
    }
}
