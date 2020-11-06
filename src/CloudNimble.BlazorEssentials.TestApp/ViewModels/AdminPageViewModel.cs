using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{

    /// <summary>
    /// 
    /// </summary>
    public class AdminPageViewModel : ViewModelBase<ConfigurationBase, AppStateBase>
    {

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="appState"></param>
        /// <param name="navigationManager"></param>
        /// <param name="httpClientFactory"></param>
        public AdminPageViewModel(ConfigurationBase configuration, AppStateBase appState, NavigationManager navigationManager, IHttpClientFactory httpClientFactory) : base(navigationManager, httpClientFactory, configuration, appState)
        {
        }

        #endregion

    }

}