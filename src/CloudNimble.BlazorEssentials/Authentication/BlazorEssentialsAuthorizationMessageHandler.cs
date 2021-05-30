using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;

namespace CloudNimble.BlazorEssentials.Authentication
{

    /// <summary>
    /// 
    /// </summary>
    public class BlazorEssentialsAuthorizationMessageHandler : AuthorizationMessageHandler
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="provider"></param>
        /// <param name="navigationManager"></param>
        public BlazorEssentialsAuthorizationMessageHandler(ConfigurationBase config, IAccessTokenProvider provider, NavigationManager navigationManager) : base(provider, navigationManager)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            ConfigureHandler(authorizedUrls: new[] { config.ApiRoot, config.AppRoot });
        }

    }

}
