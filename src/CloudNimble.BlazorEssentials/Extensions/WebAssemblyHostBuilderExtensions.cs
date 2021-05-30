using CloudNimble.BlazorEssentials;
using CloudNimble.BlazorEssentials.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Microsoft.AspNetCore.Components.WebAssembly.Hosting
{

    /// <summary>
    /// 
    /// </summary>
    public static class WebAssemblyHostBuilderExtensions
    {

        /// <summary>
        /// Registers the necessary services to bootstrap BlazorEssentials, including a <see cref="ConfigurationBase"/>, <see cref="AppStateBase"/>, and 
        /// <see cref="HttpClient">HttpClients</see> for interacting with both the 
        /// </summary>
        /// <typeparam name="TConfiguration"></typeparam>
        /// <typeparam name="TAppState"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configSectionName"></param>
        /// <returns></returns>
        public static WebAssemblyHostBuilder AddBlazorEssentials<TConfiguration, TAppState>(this WebAssemblyHostBuilder builder, string configSectionName)
            where TConfiguration : ConfigurationBase
            where TAppState : AppStateBase
        {
            return AddBlazorEssentials<TConfiguration, TAppState, BlazorEssentialsAuthorizationMessageHandler>(builder, configSectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TConfiguration"></typeparam>
        /// <typeparam name="TAppState"></typeparam>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configSectionName"></param>
        /// <returns></returns>
        /// <remarks>If your <typeparamref name="TMessageHandler"/> needs a constructor, register it before making this call.</remarks>
        public static WebAssemblyHostBuilder AddBlazorEssentials<TConfiguration, TAppState, TMessageHandler>(this WebAssemblyHostBuilder builder, string configSectionName)
            where TConfiguration : ConfigurationBase
            where TAppState : AppStateBase
            where TMessageHandler : DelegatingHandler
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(configSectionName)) throw new ArgumentNullException(nameof(configSectionName));

            var config = builder.Services.AddConfigurationBase<TConfiguration>(builder.Configuration, configSectionName);
            builder.Services.AddAppStateBase<TAppState>();
            builder.Services.AddHttpClients<TMessageHandler>(config);
            return builder;
        }

    }

}
