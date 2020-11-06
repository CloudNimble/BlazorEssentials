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

    }

}