using Downla;
using Microsoft.Extensions.DependencyInjection;
using Tengu.Business.API.Controller;
using Tengu.Business.API.Controller.Interfaces;
using Tengu.Business.API.Interfaces;
using Tengu.Business.API.Managers;
using Tengu.Business.API.Managers.Interfaces;
using Tengu.Business.Commons.Services;
using Tengu.Business.Commons.Services.Interfaces;
using Tengu.Business.Core.Adapters;
using Tengu.Business.Core.Adapters.Interfaces;
using Tengu.Business.Core.Utilities;
using Tengu.Business.Core.Utilities.Interfaces;

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
