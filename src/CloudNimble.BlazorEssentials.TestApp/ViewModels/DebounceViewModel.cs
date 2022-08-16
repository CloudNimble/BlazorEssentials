using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.EasyAF.Configuration;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class DebounceViewModel : ViewModelBase<ConfigurationBase, AppState>
    {
        #region Constructors

        public DebounceViewModel(NavigationManager navigationManager, IHttpClientFactory httpClientFactory, ConfigurationBase configuration = null, AppState appState = null) : base(navigationManager, httpClientFactory, configuration, appState)
        {
            EnableRenderCount = true;
        }

        #endregion
    }
}
