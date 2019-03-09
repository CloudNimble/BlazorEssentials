using CloudNimble.BlazorEssentials.TestApp.ViewModels;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CloudNimble.BlazorEssentials.TestApp
{

    /// <summary>
    /// Configures the startup parameters for your app and adds the required services to the DI container.
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Configures the required services for the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to populate with services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IndexViewModel>();
            services.AddSingleton<SecuredPageViewModel>();
            services.AddSingleton<AdminPageViewModel>();
            services.AddSingleton(ConfigurationHelper<ConfigurationBase>.GetConfigurationFromJson());
            services.AddSingleton(new BlazorAuthenticationConfig(
                () =>
                {
                    //RWM: Your actual code should redirect to an outside identity authority that then redirects back here.
                    var config = ConfigurationHelper<ConfigurationBase>.GetConfigurationFromJson();
                    return $"{config.AuthenticationCallbackUrl}#Secured";
                },
                (appState, token) =>
                {
                    var ci = new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, "Robert McLaws"),
                            new Claim(ClaimTypes.Role, "admin"),
                            new Claim(ClaimTypes.Expiration, DateTimeOffset.Now.AddMinutes(1).ToUnixTimeMilliseconds().ToString())
                        });
                    return new ClaimsPrincipal(ci);
                },
                (appState) => 
                {
                    //RWM: This should be a token refresh call.
                },
                (appState) =>
                {
                    //RWM: This should be a logout call.
                })
            );
            services.AddSingleton<AppStateBase>();

        }



        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }

    }

}