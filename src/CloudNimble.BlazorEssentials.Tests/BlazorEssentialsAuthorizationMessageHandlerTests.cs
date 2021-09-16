using CloudNimble.BlazorEssentials.Authentication;
using CloudNimble.Breakdance.Assemblies;
using FluentAssertions;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace CloudNimble.BlazorEssentials.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class BlazorEssentialsAuthorizationMessageHandlerTests
    {

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void BlazorEssentialsAuthorizationMessageHandler_LoadsCorrectUrls()
        {
            var config = new ConfigurationBase
            {
                ApiRoot = "https://test1",
                AppRoot = "https://test2"
            };
            var state = new BlazorEssentialsAuthorizationMessageHandler<ConfigurationBase>(config, null, null);
            state.Should().NotBeNull();

            var privateObject = new PrivateObject(state, new PrivateType(typeof(AuthorizationMessageHandler)));
            var uris = privateObject.GetField("_authorizedUris") as Uri[];
            uris.Should().NotBeNull().And.HaveCount(2);
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void BlazorEssentialsAuthorizationMessageHandler_LoadsCorrectUrls2()
        {
            var config = new SampleAdminConfigurationBase
            {
                ApiRoot = "https://test1",
                AppRoot = "https://test2"
            };
            var state = new BlazorEssentialsAuthorizationMessageHandler<SampleAdminConfigurationBase>(config, null, null);
            state.Should().NotBeNull();

            var privateObject = new PrivateObject(state, new PrivateType(typeof(AuthorizationMessageHandler)));
            var uris = privateObject.GetField("_authorizedUris") as Uri[];
            uris.Should().NotBeNull().And.HaveCount(3);
        }

    }

}
