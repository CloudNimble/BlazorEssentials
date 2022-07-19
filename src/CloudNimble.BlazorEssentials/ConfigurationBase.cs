using CloudNimble.BlazorEssentials.Authentication;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A base class implementation of the configuration your Blazor app will pull from wwwroot/appsettings.json.
    /// </summary>
    /// <remarks>
    /// This one should typically only be used for customer-facing apps.
    /// </remarks>
    public class ConfigurationBase
    {

        /// <summary>
        /// The name of the HttpClient that will be used to hit the Admin Blazor Controllers.
        /// </summary>
        public string AdminClientName { get; set; } = "AdminClient";

        /// <summary>
        /// The website your Administrative Blazor app is being served from.
        /// </summary>
        /// <remarks>
        /// Sometimes you will need to get information about the app's deployment before it has been fully-initialized in Program.cs. This is the place to do it.
        /// </remarks>
        [AuthenticatedEndpoint(nameof(AdminClientName))]
        public string AdminRoot { get; set; }

        /// <summary>
        /// The name of the HttpClient that will be used to hit the app's Public API.
        /// </summary>
        public string ApiClientName { get; set; } = "ApiClient";

        /// <summary>
        /// The root of the API that your Blazor app will call.
        /// </summary>
        /// <remarks>
        /// Most Blazor apps will call at least one API. If you need to call more than one, just inherit from ConfigurationBase and add your own properties.
        /// </remarks>
        [AuthenticatedEndpoint(nameof(ApiClientName))]
        public string ApiRoot { get; set; }

        /// <summary>
        /// The name of the HttpClient that will be used to hit the Blazor App's Controllers.
        /// </summary>
        public string AppClientName { get; set; } = "AppClient";

        /// <summary>
        /// The website your Blazor app is being served from.
        /// </summary>
        /// <remarks>
        /// Sometimes you will need to get information about the app's deployment before it has been fully-initialized in Program.cs. This is the place to do it.
        /// </remarks>
        [AuthenticatedEndpoint(nameof(AppClientName))]
        public string AppRoot { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HttpHandlerMode HttpHandlerMode { get; set; } = HttpHandlerMode.Add;

    }

}
