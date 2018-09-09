using System.Net.Http;
using Microsoft.AspNetCore.Blazor.Services;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// 
    /// </summary>
    public class ViewModelBase<T> where T : ConfigurationBase
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public T Configuration { get; internal set; }

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
        /// <param name="httpClient"></param>
        public ViewModelBase(IUriHelper uriHelper, HttpClient httpClient, T configuration)
        {
            UriHelper = uriHelper;
            HttpClient = httpClient;
            Configuration = configuration;
        }

        #endregion

    }

}