using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.EasyAF.Configuration;
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

        public IndexViewModel(ConfigurationBase configuration, AppState appState, IHttpClientFactory httpClientFactory) : base(httpClientFactory, configuration, appState)
        {
        }

        #endregion

        #region Public Methods

        public async Task Load()
        {
            try
            {
                LoadingStatus = LoadingStatus.Loading;
                var client = HttpClientFactory.CreateClient(Configuration.ApiClientName);
                Console.WriteLine(await client.GetStringAsync(Configuration.ApiRoot));
                LoadingStatus = LoadingStatus.Loaded;
            }
            catch (Exception ex)
            {
                LoadingStatus = LoadingStatus.Failed;
                Console.WriteLine(ex.Message);
            }
        }

    }

    #endregion

}