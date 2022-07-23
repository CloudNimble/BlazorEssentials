using CloudNimble.BlazorEssentials.Authentication;
using CloudNimble.Breakdance.Assemblies;
using CloudNimble.Breakdance.Blazor;
using CloudNimble.EasyAF.Configuration;
using CloudNimble.EasyAF.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Reflection;

namespace CloudNimble.BlazorEssentials.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class IServiceCollectionExtensionsTests
    {

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AddHttpClients_LoadsCorrectUrls()
        {
            var services = new ServiceCollection();
            var config = new ConfigurationBase
            {
                ApiRoot = "https://test1",
                AppRoot = "https://test2"
            };
            services.AddSingleton(config);
            services.AddHttpClients<ConfigurationBase, BlazorEssentialsAuthorizationMessageHandler<ConfigurationBase>>(config, HttpHandlerMode.None);

            var provider = services.BuildServiceProvider();

            var factory = provider.GetService<IHttpClientFactory>();
            factory.Should().NotBeNull();

            var client1 = factory.CreateClient(config.ApiClientName);
            client1.Should().NotBeNull();
            client1.BaseAddress.Should().Be(config.ApiRoot);

            var client2 = factory.CreateClient(config.AppClientName);
            client2.Should().NotBeNull();
            client2.BaseAddress.Should().Be(config.AppRoot);
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void BlazorEssentialsAuthorizationMessageHandler_LoadsCorrectUrls2()
        {
            var services = new ServiceCollection();
            var config = new SampleAdminConfigurationBase
            {
                ApiRoot = "https://test1",
                AppRoot = "https://test2"
            };
            services.AddSingleton(config);
            services.AddHttpClients<SampleAdminConfigurationBase, BlazorEssentialsAuthorizationMessageHandler<SampleAdminConfigurationBase>>(config, HttpHandlerMode.None);

            var provider = services.BuildServiceProvider();

            var factory = provider.GetService<IHttpClientFactory>();
            factory.Should().NotBeNull();

            var client1 = factory.CreateClient(config.AdminApiClientName);
            client1.Should().NotBeNull();
            client1.BaseAddress.Should().Be(config.AdminApiRoot);
        }

    }

}
