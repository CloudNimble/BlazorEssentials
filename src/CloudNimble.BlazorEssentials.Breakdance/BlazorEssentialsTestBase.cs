using CloudNimble.BlazorEssentials.Authentication;
using CloudNimble.Breakdance.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace CloudNimble.BlazorEssentials.Breakdance
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    /// <typeparam name="TAppState"></typeparam>
    public class BlazorEssentialsTestBase<TConfiguration, TAppState> : BlazorBreakdanceTestBase
            where TConfiguration : ConfigurationBase
            where TAppState : AppStateBase
    {

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public BlazorEssentialsTestBase() : base()
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configSectionName"></param>
        public void AssemblySetup(string configSectionName)
        {
            AssemblySetup<BlazorEssentialsAuthorizationMessageHandler>(configSectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="configSectionName"></param>
        public void AssemblySetup<TMessageHandler>(string configSectionName)
             where TMessageHandler : DelegatingHandler
        {
            TestHostBuilder.ConfigureServices((builder, services) => {
                var config = services.AddConfigurationBase<TConfiguration>(builder.Configuration, configSectionName);
                services.AddAppStateBase<TAppState>();
                services.AddHttpClients<TMessageHandler>(config, config.HttpHandlerMode);
                services.AddSingleton<NavigationManager, TestableNavigationManager>();
            });

            base.AssemblySetup();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="configSectionName"></param>
        /// <param name="httpHandlerMode"></param>
        public void AssemblySetup<TMessageHandler>(string configSectionName, HttpHandlerMode httpHandlerMode)
             where TMessageHandler : DelegatingHandler
        {
            TestHostBuilder.ConfigureServices((builder, services) => {
                var config = services.AddConfigurationBase<TConfiguration>(builder.Configuration, configSectionName);
                services.AddAppStateBase<TAppState>();
                services.AddHttpClients<TMessageHandler>(config, httpHandlerMode);
                services.AddSingleton<NavigationManager, TestableNavigationManager>();
            });

            base.AssemblySetup();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TestSetup(string configSectionName)
        {
            base.TestSetup();
            var config = BUnitTestContext.Services.AddConfigurationBase<TConfiguration>(TestHost.Services.GetService<IConfiguration>(), configSectionName);
            BUnitTestContext.Services.AddAppStateBase<TAppState>();
            BUnitTestContext.Services.AddHttpClients<BlazorEssentialsAuthorizationMessageHandler>(config, config.HttpHandlerMode);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TestSetup<TMessageHandler>(string configSectionName, HttpHandlerMode httpHandlerMode = HttpHandlerMode.Replace)
            where TMessageHandler : DelegatingHandler
        {
            base.TestSetup();
            var config = BUnitTestContext.Services.AddConfigurationBase<TConfiguration>(TestHost.Services.GetService<IConfiguration>(), configSectionName);
            BUnitTestContext.Services.AddAppStateBase<TAppState>();
            BUnitTestContext.Services.AddHttpClients<TMessageHandler>(config, httpHandlerMode);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// DO NOT USE THIS CONSTRUCTOR. Throws a <see cref="NotSupportedException"/> when called. You must call one of the other constructors instead.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws a NotSupportedException when called.</exception>
        public override void AssemblySetup()
        {
            throw new NotSupportedException("Use of this constructor is not supported. You must, at minimum, specify the name of the ConfigurationSection containing the " +
                "BlazorEssentials configuration parameters.");
        }

        /// <summary>
        /// DO NOT USE THIS CONSTRUCTOR. Throws a <see cref="NotSupportedException"/> when called. You must call one of the other constructors instead.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws a NotSupportedException when called.</exception>
        public override void TestSetup()
        {
            throw new NotSupportedException("Use of this constructor is not supported. You must, at minimum, specify the name of the ConfigurationSection containing the " +
                "BlazorEssentials configuration parameters.");
        }

        #endregion

    }

}
