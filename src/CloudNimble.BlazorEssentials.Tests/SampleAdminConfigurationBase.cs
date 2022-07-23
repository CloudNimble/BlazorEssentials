using CloudNimble.BlazorEssentials.Authentication;
using CloudNimble.EasyAF.Configuration;

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
        [HttpEndpoint(nameof(AdminApiClientName))]
        public string AdminApiRoot => $"{ApiRoot}Admin";

    }

}
