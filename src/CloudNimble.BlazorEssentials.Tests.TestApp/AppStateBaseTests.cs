using CloudNimble.BlazorEssentials.Breakdance;
using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.BlazorEssentials.TestApp.ViewModels;
using CloudNimble.EasyAF.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.BlazorEssentials.Tests.TestApp
{

    /// <summary>
    /// Tests the functionality of the <see cref="LoadingContainerViewModel"/>.
    /// </summary>
    /// <remarks>RWM: This is how developers should structure their tests.</remarks>
    [TestClass]
    public class AppStateBaseTests : BlazorEssentialsTestBase<ConfigurationBase, AppState>
    {

        #region Test Lifecycle

        [TestInitialize]
        public void Setup()
        {
            TestHostBuilder.ConfigureServices((context, services) => {
                services.AddScoped<AuthenticationStateProvider, TestableAuthenticationStateProvider>();
            });
            TestSetup("TestApp");
        }

        [TestCleanup]
        public void TearDown() => TestTearDown();

        #endregion

        /// <summary>
        /// Checks to make sure the <see cref="LoadingContainerViewModel" /> is registered with the <see cref="BUnitTestContext"/>.
        /// </summary>
        [TestMethod]
        public void AppState_IsRegistered()
        {
            var appState = GetService<AppState>();
            appState.Should().NotBeNull();
        }

        /// <summary>
        /// Checks to make sure the <see cref="LoadingContainerViewModel" /> is registered with the <see cref="BUnitTestContext"/>.
        /// </summary>
        [TestMethod]
        public void AppState_AuthenticationStateProvider_IsRegistered()
        {
            var authProvider = GetService<AuthenticationStateProvider>();
            authProvider.Should().NotBeNull();
        }

        /// <summary>
        /// Checks to make sure the <see cref="LoadingContainerViewModel" /> is registered with the <see cref="BUnitTestContext"/>.
        /// </summary>
        [TestMethod]
        public void AppState_AuthenticationStateProvider_IsNullInitially()
        {
            var appState = GetService<AppState>();
            appState.AuthenticationStateProvider.Should().BeNull();
        }

        /// <summary>
        /// Checks to make sure the <see cref="LoadingContainerViewModel" /> is registered with the <see cref="BUnitTestContext"/>.
        /// </summary>
        [TestMethod]
        public void AppState_ClaimsPrincipal_IsNullInitially()
        {
            var appState = GetService<AppState>();
            appState.ClaimsPrincipal.Should().BeNull();
        }

        /// <summary>
        /// Checks to make sure the <see cref="LoadingContainerViewModel" /> is registered with the <see cref="BUnitTestContext"/>.
        /// </summary>
        [TestMethod]
        public void AppState_AuthenticationStateProvider_CanBeSet()
        {
            var appState = GetService<AppState>();
            var auth = GetService<AuthenticationStateProvider>();

            appState.AuthenticationStateProvider = auth;
            appState.AuthenticationStateProvider.Should().NotBeNull();
        }

        /// <summary>
        /// Checks to make sure the <see cref="LoadingContainerViewModel" /> is registered with the <see cref="BUnitTestContext"/>.
        /// </summary>
        [TestMethod]
        public void AppState_RefreshClaimsPrincipal_ClaimsPrincipalNotNull()
        {
            var appState = GetService<AppState>();
            var auth = TestHost.Services.GetService<AuthenticationStateProvider>();

            appState.AuthenticationStateProvider = auth;
            appState.RefreshClaimsPrincipal();
            appState.ClaimsPrincipal.Should().NotBeNull();
        }

    }

}
