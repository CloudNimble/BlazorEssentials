using System.Net.Http;
using Microsoft.AspNetCore.Blazor.Services;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAppState"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public class ViewModelBase<TAppState, TConfig>
        where TAppState : AppStateBase
        where TConfig : ConfigurationBase
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public TAppState AppState { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public TConfig Configuration { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string FilterCriteria { get; set; }

        /// <summary>
        /// The HttpClient instance for the ViewModel.
        /// </summary>
        public HttpClient HttpClient { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public IUriHelper UriHelper { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="appState"></param>
        /// <param name="uriHelper"></param>
        /// <param name="httpClient"></param>
        public ViewModelBase(TConfig configuration, TAppState appState, IUriHelper uriHelper, HttpClient httpClient)
        {
            Configuration = configuration;
            AppState = appState;
            UriHelper = uriHelper;
            HttpClient = httpClient;
        }

        #endregion

    }

}