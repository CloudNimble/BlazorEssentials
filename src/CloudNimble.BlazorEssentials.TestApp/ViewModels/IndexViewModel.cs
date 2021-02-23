using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

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

        #region Public Methods

        public async Task Load()
        {
            await Task.FromResult(0).ConfigureAwait(false);
        }

    }

    #endregion

}