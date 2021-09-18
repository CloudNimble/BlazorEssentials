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
        /// <typeparam name="TConfig"></typeparam>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static IServiceCollection AddHttpClients<TConfig, TMessageHandler>(this IServiceCollection services, TConfig config)
            where TMessageHandler : DelegatingHandler
            where TConfig : ConfigurationBase
        {
            return AddHttpClients<TConfig, TMessageHandler>(services, config, config.HttpHandlerMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="httpHandlerMode"></param>
        /// <returns></returns>
        internal static IServiceCollection AddHttpClients<TConfig, TMessageHandler>(this IServiceCollection services, TConfig config, HttpHandlerMode httpHandlerMode)
            where TMessageHandler : DelegatingHandler
            where TConfig : ConfigurationBase
        {
            if (httpHandlerMode != HttpHandlerMode.None)
            {
                services.TryAddScoped<TMessageHandler>();
            }

            var properties = typeof(TConfig).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(c => c.GetCustomAttributes<AuthenticatedEndpointAttribute>().Any());

            foreach (var property in properties)
            {
                var url = property.GetValue(config) as string;
                if (string.IsNullOrWhiteSpace(url)) continue;

                var attribute = property.GetCustomAttribute<AuthenticatedEndpointAttribute>();

                services.AddHttpClient(typeof(TConfig).GetProperty(attribute.ClientNameProperty).GetValue(config) as string, client => client.BaseAddress = new Uri(url))
                    .AddHttpMessageHandler<TMessageHandler>(httpHandlerMode);
            }

            return services;
        }

    }

}
