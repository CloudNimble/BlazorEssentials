using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;

namespace CloudNimble.BlazorEssentials.TestApp
{

    /// <summary>
    /// 
    /// </summary>
    public class TestAppAuthenticationConfig : BlazorAuthenticationConfig
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationBase"></param>
        /// <param name="navigationManager"></param>
        /// <param name="httpClient"></param>
        public TestAppAuthenticationConfig(ConfigurationBase configurationBase, NavigationManager navigationManager, HttpClient httpClient)
        {
            this.GenerateRedirectUrl = () =>
            {
                //RWM: Your actual code should redirect to an outside identity authority that then redirects back here.
                return $"{configurationBase.AuthenticationCallbackUrl}#Secured";

            };

            this.ProcessToken = (appState, hash) =>
            {
                var ci = new ClaimsIdentity(
                    new List<Claim>
                    {
                            new Claim(ClaimTypes.Name, "Robert McLaws"),
                            new Claim(ClaimTypes.Role, "admin"),
                            new Claim(ClaimTypes.Expiration, DateTimeOffset.Now.AddMinutes(1).ToUnixTimeMilliseconds().ToString())
                    });
                return new ClaimsPrincipal(ci);
            };

            this.RefreshToken = (appState) =>
            {
            };

            this.SignOut = (appState) =>
            {
            };

        }

    }

}