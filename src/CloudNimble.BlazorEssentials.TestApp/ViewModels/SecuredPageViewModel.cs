using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class SecuredPageViewModel : ViewModelBase<ConfigurationBase, AppStateBase>
    {

        #region Constructors

        public SecuredPageViewModel(ConfigurationBase configuration, AppStateBase appState, IUriHelper uriHelper, HttpClient httpClient) : base(uriHelper, httpClient, configuration, appState)
        {
        }

        #endregion

    }

}