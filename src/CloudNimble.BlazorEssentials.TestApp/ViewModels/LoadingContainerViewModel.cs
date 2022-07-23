using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.EasyAF.Configuration;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class LoadingContainerViewModel : ViewModelBase<ConfigurationBase, AppState>
    {

        #region Properties

        public KeyValuePair<string, string>? Item { get; set; }

        public List<KeyValuePair<string, string>> Items { get; set; }

        public List<KeyValuePair<string, string>> NoItems { get; set; }

        #endregion

        #region Constructors

        public LoadingContainerViewModel(ConfigurationBase configuration, AppState appState, NavigationManager navigationManager, IHttpClientFactory httpClientFactory) : base(navigationManager, httpClientFactory, configuration, appState)
        {
        }

        #endregion

        #region Public Methods

        public async Task Load()
        {
            LoadingStatus = LoadingStatus.Loading;
            await Task.Delay(5000);
            Item = new ("Key", "Value");
            Items = new ()
            {
                new ("Key1", "Value1"),
                new ("Key2", "Value2")
            };
            NoItems = new();
            LoadingStatus = LoadingStatus.Loaded;
            await Task.Delay(5000);
            LoadingStatus = LoadingStatus.Failed;
        }

    }

    #endregion

}