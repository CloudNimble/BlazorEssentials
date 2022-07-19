using CloudNimble.BlazorEssentials.Authentication;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A base class implementation of the configuration your Admin Blazor app will pull from wwwroot/appsettings.json.
    /// </summary>
    /// <remarks>
    /// This one should typically only be used in Administrative console apps.
    /// </remarks>
    public class AdminConfigurationBase : ConfigurationBase
    {

        /// <summary>
        /// The name of the HttpClient that will be used to hit the Admin (Private) API.
        /// </summary>
        public string AdminApiClientName { get; set; } = "AdminApiClient";

        /// <summary>
        /// The root of the Admin (Private) API.
        /// </summary>
        /// <remarks>
        /// Most Blazor apps will call at least one API. If you need to call more than one, just inherit from ConfigurationBase and add your own properties.
        /// </remarks>
        [AuthenticatedEndpoint(nameof(AdminApiClientName))]
        public string AdminApiRoot { get; set; }

    }

}
