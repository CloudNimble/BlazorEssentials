using CloudNimble.BlazorEssentials.Breakdance;
using CloudNimble.EasyAF.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.BlazorEssentials.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ViewModeBaseTests : BlazorEssentialsTestBase<ConfigurationBase, AppStateBase>
    {

        public ViewModeBaseTests()
        {
            ClassSetup("TestApp");
        }

        #region Test Lifecycle

        [TestInitialize]
        public void TestInitialize()
        {
            TestSetup("TestApp", environment: "Production");
            BUnitTestContext.Services.AddSingleton<ViewModelBase<ConfigurationBase, AppStateBase>>();
        }

        [TestCleanup]
        public void TearDown() => TestTearDown();

        #endregion

        /// <summary>
        /// Tests that a <see cref="ViewModelBase"/> can access the environment from the <see cref="AppStateBase"/>.
        /// </summary>
        [TestMethod]
        public void ViewModelBase_Environment_Accessible()
        {
            var viewModel = GetService<ViewModelBase<ConfigurationBase, AppStateBase>>();

            viewModel.Should().NotBeNull();
            viewModel.AppState.Environment.Should().NotBeNull();
            viewModel.AppState.Environment.Environment.Should().Be("Production");
            viewModel.AppState.Environment.IsProduction();
        }

    }

}
