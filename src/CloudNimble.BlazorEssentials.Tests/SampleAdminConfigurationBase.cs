using CloudNimble.BlazorEssentials.Authentication;

namespace CloudNimble.BlazorEssentials.Tests
{


    public class SampleAdminConfigurationBase : ConfigurationBase
    {

        /// <summary>
        /// 
        /// </summary>
        public string AdminApiClientName => $"Admin{ApiClientName}";

        /// <summary>
        /// 
        /// </summary>
        [AuthenticatedEndpoint]
        public string AdminApiRoot => $"{ApiRoot}Admin";

    }

}
