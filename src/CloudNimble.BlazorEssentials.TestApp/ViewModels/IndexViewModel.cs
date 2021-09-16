using CloudNimble.BlazorEssentials.TestApp.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class IndexViewModel : ViewModelBase<ConfigurationBase, AppState>
    {

        #region Properties


        #endregion

        #region Constructors

        public IndexViewModel(ConfigurationBase configuration, AppState appState, NavigationManager navigationManager, IHttpClientFactory httpClientFactory) : base(navigationManager, httpClientFactory, configuration, appState)
        {
        }

        #endregion

        #region Public Methods

        public async Task Load()
        {
            LoadingStatus = LoadingStatus.Loading;
            var client = HttpClientFactory.CreateClient(Configuration.ApiClientName);
            Console.WriteLine(await client.GetStringAsync(Configuration.ApiRoot));
            LoadingStatus = LoadingStatus.Loaded;
        }

    }

    #endregion

}