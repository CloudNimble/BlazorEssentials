using CloudNimble.BlazorEssentials.Navigation;
using CloudNimble.EasyAF.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System;
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
        public AppState(NavigationManager navigationManager, IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime, IWebAssemblyHostEnvironment environment, NavigationHistory navHistory, ConfigurationBase config)
            : base(navigationManager, httpClientFactory, jsRuntime, environment, navHistory)
        {
            if (!Environment.IsProduction())
            {
                StateHasChanged.DebugMode = StateHasChangedDebugMode.Info;
            }
            StateHasChanged.DelayMode = StateHasChangedDelayMode.Throttle;
            StateHasChanged.DelayInterval = 100;

            this.config = config;
            var nav = new List<NavigationItem>
            {
                new NavigationItem("Dashboard",             "fas fa-fw fa-lg fa-desktop",       "/",                    "Main",             true, "Dashboard",              "fad fa-fw fa-2x fa-desktop",   null, true),
                new NavigationItem("LoadingContainer Demo", "fas fa-fw fa-lg fa-spinner",       "LoadingContainer",     "Controls",         true, "LoadingContainer Demo",  "fad fa-fw fa-2x fa-desktop",   null, true),
                new NavigationItem("Merlin Demo",           "fas fa-fw fa-lg fa-hat-wizard",    "Merlin",               "Controls",         true, "Merlin Demo",            "fad fa-fw fa-2x fa-desktop",   null, true),
                new NavigationItem("Delay StateHasChanged", "fas fa-fw fa-lg fa-bolt",          "DelayStateHasChanged", "Functionality",    true, "Delay StateHasChanged",  "fad fa-fw fa-2x fa-desktop",   null, true),
                new NavigationItem("IndexedDB",             "fas fa-fw fa-lg fa-database",      "IndexedDb",            "IndexedDB",        true, "IndexedDB",              "fad fa-fw fa-2x fa-database",  null, true)
            };
            LoadNavItems(nav);
            PropertyChanged += AppState_PropertyChanged;
        }

        #endregion

        #region Event Handlers

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        private void AppState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine($"AppState.{e.PropertyName} changed.");
            StateHasChanged.Action();
        }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        #endregion

    }


}
