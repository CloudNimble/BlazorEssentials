using System.Net.Http;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// 
    /// </summary>
    public class ViewModelBase<T> where T : ConfigurationBase
    {

        #region Properties

        /// <summary>
        /// The HttpClient instance for the ViewModel.
        /// </summary>
        public HttpClient HttpClient { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public T Configuration { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string FilterCriteria { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        public ViewModelBase(HttpClient httpClient, T configuration)
        {
            HttpClient = httpClient;
            Configuration = configuration;
        }

        #endregion

    }

}