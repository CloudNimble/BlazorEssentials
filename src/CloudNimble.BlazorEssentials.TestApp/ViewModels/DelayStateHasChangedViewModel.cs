using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.EasyAF.Configuration;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{

    public class DelayStateHasChangedViewModel : ViewModelBase<ConfigurationBase, AppState>
    {

        #region Constructors

        public DelayStateHasChangedViewModel(IHttpClientFactory httpClientFactory, ConfigurationBase configuration = null, AppState appState = null) : base(httpClientFactory, configuration, appState)
        {
            StateHasChanged.DebugMode = StateHasChangedDebugMode.Tuning;
            appState.StateHasChanged.DebugMode = StateHasChangedDebugMode.Tuning;
        }

        #endregion

    }

}
