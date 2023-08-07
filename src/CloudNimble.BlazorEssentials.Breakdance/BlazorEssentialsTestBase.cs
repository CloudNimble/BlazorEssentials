using CloudNimble.BlazorEssentials.Authentication;
using CloudNimble.BlazorEssentials.Navigation;
using CloudNimble.Breakdance.Blazor;
using CloudNimble.EasyAF.Configuration;
using CloudNimble.EasyAF.Core;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        public void ClassSetup(string configSectionName)
        {
            ClassSetup<BlazorEssentialsAuthorizationMessageHandler<TConfiguration>>(configSectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="configSectionName"></param>
        /// <param name="environment"></param>
        /// <param name="baseAddress"></param>
        public void ClassSetup<TMessageHandler>(string configSectionName, string environment = "Development", string baseAddress = "https://localhost")
             where TMessageHandler : DelegatingHandler
        {
            TestHostBuilder.AddBlazorEssentials<TConfiguration, TAppState, TMessageHandler>(configSectionName);

            TestHostBuilder.ConfigureServices((builder, services) => {
                services.AddSingleton<IWebAssemblyHostEnvironment, TestableWebAssemblyHostEnvironment>(sp => new TestableWebAssemblyHostEnvironment(environment, baseAddress));
            });

            base.ClassSetup();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="configSectionName"></param>
        /// <param name="httpHandlerMode"></param>
        /// <param name="environment"></param>
        /// <param name="baseAddress"></param>
        public void ClassSetup<TMessageHandler>(string configSectionName, HttpHandlerMode httpHandlerMode, string environment = "Development", string baseAddress = "https://localhost")
             where TMessageHandler : DelegatingHandler
        {
            TestHostBuilder.AddBlazorEssentials<TConfiguration, TAppState, TMessageHandler>(configSectionName, httpHandlerMode);

            TestHostBuilder.ConfigureServices((builder, services) => {
                services.AddSingleton<IWebAssemblyHostEnvironment, TestableWebAssemblyHostEnvironment>(sp => new TestableWebAssemblyHostEnvironment(environment, baseAddress));
            });

            base.ClassSetup();
        }

        /// <summary>
        /// Configures the BlazorEssentials services into the BUnitTestContext IServiceProvider for the currently-executing test only.
        /// </summary>
        /// <param name="configSectionName"></param>
        /// <param name="environment"></param>
        /// <param name="baseAddress"></param>
        /// <remarks>
        /// RWM: These methods exist because bUnit is configured per-test, and the BlazorEssentials configuration can change on a per-test basis.
        /// bUnit will resolve from its own container first, then fall back to the TestHost's ServiceProvider if not found. This methods puts a new configuration in place
        /// instead, to be used only for the currently-executing test.
        /// </remarks>
        public void TestSetup(string configSectionName, string environment = "Development", string baseAddress = "https://localhost")
        {
            base.TestSetup();
            BUnitTestContext.Services.AddSingleton<NavigationHistory>();
            var config = BUnitTestContext.Services.AddConfigurationBase<TConfiguration>(TestHost.Services.GetService<IConfiguration>(), configSectionName);
            BUnitTestContext.Services.AddAppStateBase<TAppState>();
            BUnitTestContext.Services.AddHttpClients<TConfiguration, BlazorEssentialsAuthorizationMessageHandler<TConfiguration>>(config, config.HttpHandlerMode);
            BUnitTestContext.Services.AddSingleton<IWebAssemblyHostEnvironment, TestableWebAssemblyHostEnvironment>(sp => new TestableWebAssemblyHostEnvironment(environment, baseAddress));
        }

        /// <summary>
        /// Configures the BlazorEssentials services into the BUnitTestContext IServiceProvider for the currently-executing test only.
        /// </summary>
        /// <typeparam name="TMessageHandler"></typeparam>
        /// <param name="configSectionName"></param>
        /// <param name="httpHandlerMode"></param>
        /// <param name="environment"></param>
        /// <param name="baseAddress"></param>
        /// <remarks>
        /// RWM: These methods exist because bUnit is configured per-test, and the BlazorEssentials configuration can change on a per-test basis.
        /// bUnit will resolve from its own container first, then fall back to the TestHost's ServiceProvider if not found. This methods puts a new configuration in place
        /// instead, to be used only for the currently-executing test.
        /// </remarks>
        public void TestSetup<TMessageHandler>(string configSectionName, HttpHandlerMode httpHandlerMode = HttpHandlerMode.Replace, string environment = "Development", string baseAddress = "https://localhost")
            where TMessageHandler : DelegatingHandler
        {
            base.TestSetup();
            BUnitTestContext.Services.AddSingleton<NavigationHistory>();
            var config = BUnitTestContext.Services.AddConfigurationBase<TConfiguration>(TestHost.Services.GetService<IConfiguration>(), configSectionName);
            BUnitTestContext.Services.AddAppStateBase<TAppState>();
            BUnitTestContext.Services.AddHttpClients<TConfiguration, TMessageHandler>(config, httpHandlerMode);
            BUnitTestContext.Services.AddSingleton<IWebAssemblyHostEnvironment, TestableWebAssemblyHostEnvironment>(sp => new TestableWebAssemblyHostEnvironment(environment));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// DO NOT USE THIS METHOD. Throws a <see cref="NotSupportedException"/> when called. You must call <see cref="ClassSetup(string)"/> 
        /// or <see cref="TestSetup()" /> instead.
        /// </summary>
        /// <exception cref="NotSupportedException">Throws a NotSupportedException when called.</exception>
        public override void AssemblySetup()
        {
            throw new NotSupportedException("Use of this constructor is not supported. You must, at minimum, specify the name of the ConfigurationSection containing the " +
                "BlazorEssentials configuration parameters.");
        }

        /// <summary>
        /// DO NOT USE THIS METHOD. Throws a <see cref="NotSupportedException"/> when called. You must call <see cref="ClassSetup(string)"/> 
        /// or <see cref="TestSetup()" /> instead.
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
