using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.EasyAF.Configuration;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class DelayStateHasChangedViewModel : ViewModelBase<ConfigurationBase, AppState>
    {
        #region Constructors

        public DelayStateHasChangedViewModel(NavigationManager navigationManager, IHttpClientFactory httpClientFactory, ConfigurationBase configuration = null, AppState appState = null) : base(navigationManager, httpClientFactory, configuration, appState)
        {
            DebugMode = true;
            appState.DebugMode = true;
        }

        #endregion
    }
}
