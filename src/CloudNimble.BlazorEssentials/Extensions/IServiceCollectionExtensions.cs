using CloudNimble.BlazorEssentials;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;

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
        /// <typeparam name="TConfiguration"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="configSectionName"></param>
        /// <returns></returns>
        internal static TConfiguration AddConfigurationBase<TConfiguration>(this IServiceCollection services, IConfiguration configuration, string configSectionName)
            where TConfiguration : ConfigurationBase
        {
            var config = configuration.GetSection(configSectionName).Get<TConfiguration>();
            services.AddSingleton(c => config);

            if (typeof(TConfiguration) != typeof(ConfigurationBase))
            {
                services.AddSingleton(sp => sp.GetRequiredService<TConfiguration>() as ConfigurationBase);
            }
            return config;
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static IServiceCollection AddHttpClients<TMessageHandler>(this IServiceCollection services, ConfigurationBase config)
            where TMessageHandler : DelegatingHandler
        {
            return AddHttpClients<TMessageHandler>(services, config, config.HttpHandlerMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="httpHandlerMode"></param>
        /// <returns></returns>
        internal static IServiceCollection AddHttpClients<TMessageHandler>(this IServiceCollection services, ConfigurationBase config, HttpHandlerMode httpHandlerMode)
            where TMessageHandler : DelegatingHandler
        {
            services.TryAddScoped<TMessageHandler>();

            if (!string.IsNullOrWhiteSpace(config.AppRoot))
            {
                services.AddHttpClient(config.AppClientName, client => client.BaseAddress = new Uri(config.AppRoot))
                    .AddHttpMessageHandler<TMessageHandler>(httpHandlerMode);
            }

            if (!string.IsNullOrWhiteSpace(config.ApiRoot))
            {
                services.AddHttpClient(config.ApiClientName, client => client.BaseAddress = new Uri(config.ApiRoot))
                        .AddHttpMessageHandler<TMessageHandler>(httpHandlerMode);
            }

            return services;
        }

    }

}
