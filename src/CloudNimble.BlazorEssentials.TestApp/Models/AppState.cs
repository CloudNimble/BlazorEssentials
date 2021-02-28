using CloudNimble.BlazorEssentials.Navigation;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials.TestApp.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class AppState : AppStateBase
    {
        #region Private Members

        private readonly ConfigurationBase config;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationManager"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="config"></param>
        public AppState(NavigationManager navigationManager, IHttpClientFactory httpClientFactory, ConfigurationBase config) : base(navigationManager, httpClientFactory)
        {
            //RWM: We don't do this here because this is Singleton scoped and the AuthProvider is not
            //this.AuthenticationStateProvider = authStateProvider;
            this.config = config;
            var nav = new List<NavigationItem>
            {
                new NavigationItem("Dashboard",             "fad fa-fw fa-desktop", "/",                    "Main",        true, "Dashboard",              "fad fa-fw fa-2x fa-desktop", null, true),
                new NavigationItem("LoadingContainer Demo", "fad fa-fw fa-desktop", "/LoadingContainer",    "Controls",    true, "LoadingContainer Demo",  "fad fa-fw fa-2x fa-desktop", null, true)
            };
            LoadNavItems(nav);

            this.PropertyChanged += AppState_PropertyChanged;
        }

        #endregion

        #region Event Handlers

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        private void AppState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //case nameof(AuthenticationStateProvider):
                //    this.AuthenticationStateProvider.AuthenticationStateChanged += AuthenticationStateProvider_AuthenticationStateChanged;
                //break;
                //case nameof(SelectedCompany):
                //    LoadCompanyDetails();
                //    break;
            }
            //RWM: Can't do this here because the handler is not async.
            StateHasChangedAction();
        }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        #endregion

    }


}
