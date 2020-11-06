using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Security.Claims;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class IndexViewModel : ViewModelBase<ConfigurationBase, AppStateBase>
    {

        #region Properties



        #endregion


        #region Constructors

        public IndexViewModel(ConfigurationBase configuration, AppStateBase appState, NavigationManager navigationManager, IHttpClientFactory httpClientFactory) : base(navigationManager, httpClientFactory, configuration, appState)
        {
        }

        #endregion

    }

}