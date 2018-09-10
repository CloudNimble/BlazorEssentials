using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CloudNimble.BlazorEssentials.TestApp
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(ConfigurationHelper<ConfigurationBase>.GetConfigurationFromJson());
            services.AddSingleton(new AppStateBase(
                () =>
                {
                    return "";
                },
                (appState, token) =>
                {
                    var ci = new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Expiration, DateTimeOffset.Now.AddMinutes(1).ToUnixTimeMilliseconds().ToString())
                        });
                    return new ClaimsPrincipal(ci);
                },
                (appState) => 
                {
                    appState.CurrentUser = null;
                },
                (appState) =>
                {
                    var ci = new ClaimsIdentity(
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Expiration, DateTimeOffset.Now.AddMinutes(1).ToUnixTimeMilliseconds().ToString())
                        });
                    appState.CurrentUser = new ClaimsPrincipal(ci);
                })
            );
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }

    }

}