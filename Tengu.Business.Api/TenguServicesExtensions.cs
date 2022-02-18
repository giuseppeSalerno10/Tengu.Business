using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Core;

namespace Tengu.Business.API
{
    public static class TenguServicesExtensions
    {
        public static IServiceCollection AddTenguServices(this IServiceCollection services)
        {
            services.AddSingleton<ITenguApi, TenguApi>();

            services.AddTransient<IAnimeSaturnManager, AnimeSaturnManager>();
            services.AddTransient<IAnimeUnityManager, AnimeUnityManager>();

            services.AddTransient<IAnimeSaturnAdapter, AnimeSaturnAdapter>();
            services.AddTransient<IAnimeUnityAdapter, AnimeUnityAdapter>();
            services.AddTransient<IKitsuAdapter, KitsuAdapter>();

            services.AddTransient<IAnimeSaturnUtilities, AnimeSaturnUtilities>();
            services.AddTransient<IAnimeUnityUtilities, AnimeUnityUtilities>();

            return services;
        }
    }
}
