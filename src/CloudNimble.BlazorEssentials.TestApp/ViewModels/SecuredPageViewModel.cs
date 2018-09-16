using System.Net.Http;
using Microsoft.AspNetCore.Blazor.Services;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class SecuredPageViewModel : ViewModelBase<ConfigurationBase, AppStateBase>
    {

        #region Constructors

        public SecuredPageViewModel(ConfigurationBase configuration, AppStateBase appState, IUriHelper uriHelper, HttpClient httpClient) : base(configuration, appState, uriHelper, httpClient)
        {
        }

        #endregion

    }

}