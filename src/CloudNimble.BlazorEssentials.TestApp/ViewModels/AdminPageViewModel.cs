using Microsoft.AspNetCore.Components.Services;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class AdminPageViewModel : ViewModelBase<ConfigurationBase, AppStateBase>
    {

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="appState"></param>
        /// <param name="uriHelper"></param>
        /// <param name="httpClient"></param>
        public AdminPageViewModel(ConfigurationBase configuration, AppStateBase appState, IUriHelper uriHelper, HttpClient httpClient) : base(configuration, appState, uriHelper, httpClient)
        {
            //RWM: Demonstrate setting roles from a ViewModel.
            AddRoles("admin");
        }

        #endregion

    }

}