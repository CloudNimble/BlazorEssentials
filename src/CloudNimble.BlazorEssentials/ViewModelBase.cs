using CloudNimble.EasyAF.Configuration;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A base class for your Blazor MVVM implementation that gives you access to all the useful stuff Blazor and BlazorEssentials inject into the app.
    /// </summary>
    /// <typeparam name="TAppState"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public class ViewModelBase<TConfig, TAppState> : BlazorObservable
        where TConfig : ConfigurationBase
        where TAppState : AppStateBase
    {

        #region Properties

        /// <summary>
        /// The injected <see cref="AppStateBase"/> instance for the ViewModel.
        /// </summary>
        public TAppState AppState { get; internal set; }

        /// <summary>
        /// The injected <see cref="ConfigurationBase"/> instance for the ViewModel. 
        /// </summary>
        public TConfig Configuration { get; internal set; }

        /// <summary>
        /// Allows you to set any additional filtering criteria for this ViewModels' HTTP requests from inside the Page itself.
        /// </summary>
        public string FilterCriteria { get; set; }

        /// <summary>
        /// The injected <see cref="IHttpClientFactory"/> instance for the ViewModel.
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; internal set; }

        /// <summary>
        /// The injected <see cref="NavigationManager"/> instance for the ViewModel.
        /// </summary>
        public NavigationManager NavigationManager { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ViewModelBase{TConfig, TAppState}"/>.
        /// </summary>
        /// <param name="navigationManager">The <see cref="NavigationManager"/> instance injected from the DI container.</param>
        /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/> instance injected from the DI container.</param>
        /// <param name="configuration">The <typeparamref name="TConfig"/> instance injected from the DI container.</param>
        /// <param name="appState">The <typeparamref name="TAppState"/> instance injected from the DI container.</param>
        /// <param name="stateHasChangedConfig"></param>
        public ViewModelBase(NavigationManager navigationManager, IHttpClientFactory httpClientFactory, TConfig configuration = null, TAppState appState = null, StateHasChangedConfig stateHasChangedConfig = null)
            : base(stateHasChangedConfig)
        {
            NavigationManager = navigationManager;
            HttpClientFactory = httpClientFactory;
            Configuration = configuration;
            AppState = appState ?? (TAppState)new AppStateBase(navigationManager, httpClientFactory);
        }

        #endregion

        #region Public Methods



        #endregion

    }

}