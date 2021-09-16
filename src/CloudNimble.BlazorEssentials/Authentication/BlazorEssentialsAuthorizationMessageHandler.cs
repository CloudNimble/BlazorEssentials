using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Linq;
using System.Reflection;

namespace CloudNimble.BlazorEssentials.Authentication
{

    /// <summary>
    /// 
    /// </summary>
    public class BlazorEssentialsAuthorizationMessageHandler<T> : AuthorizationMessageHandler
        where T: ConfigurationBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="provider"></param>
        /// <param name="navigationManager"></param>
        public BlazorEssentialsAuthorizationMessageHandler(T config, IAccessTokenProvider provider, NavigationManager navigationManager) : base(provider, navigationManager)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            var authorizedUrls = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(c => c.GetCustomAttributes<AuthenticatedEndpointAttribute>().Any())
                .Select(c => c.GetValue(config))
                .Where(c => !string.IsNullOrWhiteSpace(c as string))
                .Select(c => c as string);

            ConfigureHandler(authorizedUrls: authorizedUrls);
        }

    }

}
