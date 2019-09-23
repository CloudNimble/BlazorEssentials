using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{

    /// <summary>
    /// 
    /// </summary>
    public class SecuredPageViewModel : ViewModelBase<ConfigurationBase, AppStateBase>
    {

        #region Constructors

        public SecuredPageViewModel(ConfigurationBase configuration, AppStateBase appState, NavigationManager navigationManager, HttpClient httpClient) : base(navigationManager, httpClient, configuration, appState)
        {
        }

        #endregion

    }

}