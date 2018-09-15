namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationBase
    {

        /// <summary>
        /// The root of the API that your Blazor app will call.
        /// </summary>
        /// <remarks>
        /// Most Blazor apps will call at least one API. If you need to call more than one, just inherit from ConfigurationBase and add your own properties.
        /// </remarks>
        public string ApiRoot { get; set; }

        /// <summary>
        /// The page that the Authentication system will use to tell your login provider where to redirect to in order to process your login token. Defaults to "Account/LoginCallback".
        /// </summary>
        public string AuthenticationCallbackUrl { get; set; } = "Account/LoginCallback";

        /// <summary>
        /// A string representing the page to redirect to if the User is not logged in or not in the right Role to view the page. Defaults to "Account/Unauthorized".
        /// </summary>
        public string UnauthorizedRedirectUrl { get; set; } = "Account/Unauthorized";

    }

}