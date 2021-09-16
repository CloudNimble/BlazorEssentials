using CloudNimble.BlazorEssentials.Authentication;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A base class implementation of the configuration your Blazor app will pull from wwwroot/appsettings.json.
    /// </summary>
    public class ConfigurationBase
    {

        /// <summary>
        /// 
        /// </summary>
        public string ApiClientName { get; set; } = "ApiClient";

        /// <summary>
        /// The root of the API that your Blazor app will call.
        /// </summary>
        /// <remarks>
        /// Most Blazor apps will call at least one API. If you need to call more than one, just inherit from ConfigurationBase and add your own properties.
        /// </remarks>
        [AuthenticatedEndpoint]
        public string ApiRoot { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppClientName { get; set; } = "AppClient";

        /// <summary>
        /// The website your Blazor app is being served from.
        /// </summary>
        /// <remarks>
        /// Sometimes you will need to get information about the app's deployment before it has been fully-initialized in Program.cs. This is the place to do it.
        /// </remarks>
        [AuthenticatedEndpoint]
        public string AppRoot { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpHandlerMode HttpHandlerMode { get; set; } = HttpHandlerMode.Add;

    }

}
