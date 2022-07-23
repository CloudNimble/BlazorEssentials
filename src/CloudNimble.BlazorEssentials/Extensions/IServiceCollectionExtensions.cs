using CloudNimble.BlazorEssentials;
using CloudNimble.BlazorEssentials.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// 
    /// </summary>
    internal static class IServiceCollectionExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAppState"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection AddAppStateBase<TAppState>(this IServiceCollection services)
            where TAppState : AppStateBase
        {
            services.AddSingleton<TAppState>();
            if (typeof(TAppState) != typeof(AppStateBase))
            {
                services.AddSingleton<AppStateBase>(sp => sp.GetRequiredService<TAppState>());
            }
            return services;
        }

    }

}
