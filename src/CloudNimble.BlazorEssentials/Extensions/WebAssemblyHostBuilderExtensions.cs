using CloudNimble.BlazorEssentials;
using CloudNimble.BlazorEssentials.Authentication;
using CloudNimble.BlazorEssentials.Navigation;
using CloudNimble.EasyAF.Configuration;
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
        /// <typeparam name="TConfiguration">The <see cref="ConfigurationBase"/>-derived type to register in the DI container.</typeparam>
        /// <typeparam name="TAppState">The <see cref="AppStateBase"/>-derived type to register in the DI container.</typeparam>
        /// <param name="builder">The <see cref="WebAssemblyHostBuilder"/> instance to configure.</param>
        /// <param name="configSectionName">The name of the Configuration node in appsettings.json that specifies BlazorEssentials settings.</param>
        /// <returns>The </returns>
        public static WebAssemblyHostBuilder AddBlazorEssentials<TConfiguration, TAppState>(this WebAssemblyHostBuilder builder, string configSectionName)
            where TConfiguration : ConfigurationBase
            where TAppState : AppStateBase
        {
            return AddBlazorEssentials<TConfiguration, TAppState, BlazorEssentialsAuthorizationMessageHandler<TConfiguration>>(builder, configSectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TConfiguration">The <see cref="ConfigurationBase"/>-derived type to register in the DI container.</typeparam>
        /// <typeparam name="TAppState">The <see cref="AppStateBase"/>-derived type to register in the DI container.</typeparam>
        /// <typeparam name="TMessageHandler">
        /// The <see cref="DelegatingHandler"/>-derived type to register for the built-in HttpClients. Defaults to <see cref="BlazorEssentialsAuthorizationMessageHandler{TConfiguration}"/>.
        /// </typeparam>
        /// <param name="builder">The <see cref="WebAssemblyHostBuilder"/> instance to configure.</param>
        /// <param name="configSectionName">The name of the Configuration node in appsettings.json that specifies BlazorEssentials settings.</param>
        /// <returns></returns>
        /// <remarks>If your <typeparamref name="TMessageHandler"/> needs a constructor, register it before making this call.</remarks>
        public static WebAssemblyHostBuilder AddBlazorEssentials<TConfiguration, TAppState, TMessageHandler>(this WebAssemblyHostBuilder builder, string configSectionName)
            where TConfiguration : ConfigurationBase
            where TAppState : AppStateBase
            where TMessageHandler : DelegatingHandler
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            if (string.IsNullOrWhiteSpace(configSectionName)) throw new ArgumentNullException(nameof(configSectionName), "You must specify the name of the Configuration node in appsettings.json that specifies BlazorEssentials settings.");

            var config = builder.Services.AddConfigurationBase<TConfiguration>(builder.Configuration, configSectionName);
            builder.Services.AddSingleton<NavigationHistory>();
            builder.Services.AddAppStateBase<TAppState>();
            builder.Services.AddHttpClients<TConfiguration, TMessageHandler>(config);
            return builder;
        }

    }

}
