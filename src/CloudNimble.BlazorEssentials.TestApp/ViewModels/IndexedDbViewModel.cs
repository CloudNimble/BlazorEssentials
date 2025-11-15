using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.EasyAF.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TestApp.ViewModels
{
    public class IndexedDbViewModel : ViewModelBase<ConfigurationBase, AppState>
    {

        IQueryable<ExampleEvent> exampleEvents;

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public ExampleDb ExampleDb { get; set; }

        public IQueryable<ExampleEvent> ExampleEvents
        {
            get => exampleEvents;
            set => Set(() => ExampleEvents, ref exampleEvents, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="exampleDb"></param>
        /// <param name="configuration"></param>
        /// <param name="appState"></param>
        /// <param name="httpClientFactory"></param>
        public IndexedDbViewModel(ExampleDb exampleDb, ConfigurationBase configuration, AppState appState, IHttpClientFactory httpClientFactory) : base(httpClientFactory, configuration, appState)
        {
            ExampleDb = exampleDb;
            ExampleEvents = new List<ExampleEvent>().AsQueryable();
            PropertyChanged += IndexedDbViewModel_PropertyChanged;
        }

        private void IndexedDbViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StateHasChanged.Action();
        }

        #endregion

        #region Public Methods

        public async Task Load()
        {
            try
            {
                LoadingStatus = LoadingStatus.Loading;
                ExampleEvents = (await ExampleDb.Events.GetAllAsync<ExampleEvent>()).AsQueryable();
                LoadingStatus = LoadingStatus.Loaded;
            }
            catch (Exception ex)
            {
                LoadingStatus = LoadingStatus.Failed;
                Console.WriteLine(ex.Message);
            }
        }

        public async Task LoadLimited(int count, int skip)
        {
            try
            {
                LoadingStatus = LoadingStatus.Loading;
                ExampleEvents = (await ExampleDb.Events.QueryAsync<ExampleEvent>("return obj;", count, skip)).AsQueryable();
                LoadingStatus = LoadingStatus.Loaded;
            }
            catch (Exception ex)
            {
                LoadingStatus = LoadingStatus.Failed;
                Console.WriteLine(ex.Message);
            }
        }

        public async Task AddEntry()
        {
            var newEvent = new ExampleEvent()
            {
                Name = "New Event" + Random.Shared.Next(1, 100),
                Description = DateTimeOffset.Now.ToString()
            };
            await ExampleDb.Events.AddAsync(newEvent);
            await Load();
        }

    }

    #endregion

}