﻿using CloudNimble.BlazorEssentials;
using CloudNimble.EasyAF.Configuration;
using CloudNimble.EasyAF.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.Hosting
{

    /// <summary>
    /// A class for extending an <see cref="IHostBuilder"/> instance.
    /// </summary>
    public static class IHostBuilderExtensions
    {

        /// <summary>
        /// Adds Blazor capabilities to the provided <see cref="IHostBuilder"/>.
        /// </summary>
        /// <typeparam name="TConfiguration"></typeparam>
        /// <typeparam name="TAppState"></typeparam>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configSectionName"></param>
        /// <returns></returns>
        public static IHostBuilder AddBlazorEssentials<TConfiguration, TAppState, TMessageHandler>(this IHostBuilder builder, string configSectionName)
            where TConfiguration : ConfigurationBase
            where TAppState : AppStateBase
            where TMessageHandler : DelegatingHandler
        {
            return AddBlazorEssentials<TConfiguration, TAppState, TMessageHandler>(builder, configSectionName, HttpHandlerMode.Add);
        }

        /// <summary>
        /// Adds Blazor capabilities to the provided <see cref="IHostBuilder"/>.
        /// </summary>
        /// <typeparam name="TConfiguration"></typeparam>
        /// <typeparam name="TAppState"></typeparam>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configSectionName"></param>
        /// <param name="httpHandlerMode"></param>
        /// <returns></returns>
        public static IHostBuilder AddBlazorEssentials<TConfiguration, TAppState, TMessageHandler>(this IHostBuilder builder, string configSectionName, HttpHandlerMode httpHandlerMode)
            where TConfiguration : ConfigurationBase
            where TAppState : AppStateBase
            where TMessageHandler : DelegatingHandler
        {
            builder.ConfigureServices((builder, services) => 
            {
                var config = services.AddConfigurationBase<TConfiguration>(builder.Configuration, configSectionName);
                if (config is null)
                {
                    throw new ArgumentException($"The ConfigurationSection '{configSectionName}' could not be found.", nameof(configSectionName));
                }
                services.AddAppStateBase<TAppState>();
                services.AddHttpClients<TConfiguration, TMessageHandler>(config, httpHandlerMode);
            });

            return builder;
        }

    }

}
