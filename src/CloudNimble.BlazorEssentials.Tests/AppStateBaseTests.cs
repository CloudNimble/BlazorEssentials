using CloudNimble.BlazorEssentials.Breakdance;
using CloudNimble.BlazorEssentials.Extensions;
using CloudNimble.BlazorEssentials.Navigation;
using CloudNimble.EasyAF.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class AppStateBaseTests : BlazorEssentialsTestBase<ConfigurationBase, AppStateBase>
    {

        #region Private Members

        Func<Task<bool>> trueAction = () => { Thread.Sleep(2000); return Task.FromResult(true); };
        Func<Task<bool>> falseAction = () => { Thread.Sleep(2000); return Task.FromResult(false); };

        #endregion

        public AppStateBaseTests()
        {
            ClassSetup("TestApp");
        }

        #region Test Lifecycle

        [TestInitialize]
        public void TestInitialize()
        {
            TestSetup("TestApp", environment: "Production");
        }

        [TestCleanup]
        public void TearDown() => TestTearDown();

        #endregion

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_CorrectInitialState()
        {
            var state = new AppStateBase(null, null, null, null, null);
            state.LoadingStatus.Should().Be(LoadingStatus.NotLoaded);
            state.NavItems.Should().BeNull();
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_SetCurrentNavItem_Nested_NoParameters()
        {
            var state = GetService<AppStateBase>();

            var list = new List<NavigationItem>
            {
                new NavigationItem("Test1", "Icon", "Category1", true, new List<NavigationItem>
                {
                    new NavigationItem("Inner1", "Icon1", "/")
                }),
                new NavigationItem("Test2", "Icon", "Category1", true, new List<NavigationItem>
                {
                    new NavigationItem("Inner2", "Icon2", "2"),
                    new NavigationItem("Inner3", "Icon3", "3")
                }),
            };

            list.Should().HaveCount(2);

            state.LoadNavItems(list);
            state.NavItems.Should().HaveCount(2);

            var results = state.NavItems.Traverse(c => c.Children);
            results.Should().HaveCount(5);
            results.First().Text.Should().Be("Test1");

            state.SetCurrentNavItem("2");

            state.CurrentNavItem.Should().NotBeNull();
            state.CurrentNavItem.Text.Should().Be("Inner2");
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_SetCurrentNavItem_Nested_Parameters()
        {
            var state = GetService<AppStateBase>();

            var list = new List<NavigationItem>
            {
                new NavigationItem("Test1", "Icon", "Category1", true, new List<NavigationItem>
                {
                    new NavigationItem("Inner1", "Icon1", "/")
                }),
                new NavigationItem("Test2", "Icon", "Category1", true, new List<NavigationItem>
                {
                    new NavigationItem("Inner2", "Icon2", "2"),
                    new NavigationItem("Inner3", "Icon3", "3")
                }),
            };

            list.Should().HaveCount(2);

            state.LoadNavItems(list);
            state.NavItems.Should().HaveCount(2);

            var results = state.NavItems.Traverse(c => c.Children);
            results.Should().HaveCount(5);
            results.First().Text.Should().Be("Test1");

            state.SetCurrentNavItem("2/SomeParameter");

            state.CurrentNavItem.Should().NotBeNull();
            state.CurrentNavItem.Text.Should().Be("Inner2");
        }

        /// <summary>
        /// Tests that the <see cref="IWebAssemblyHostEnvironment" /> is set correctly on the <see cref="AppState"/>.
        /// </summary>
        [TestMethod]
        public void AppStateBase_Environment_IsSetCorrectly()
        {
            var appState = GetService<AppStateBase>();

            appState.Environment.Should().NotBeNull();
            appState.Environment.Environment.Should().Be("Production");
            appState.Environment.IsProduction().Should().BeTrue();
        }

        #region ToFixedUrl

        /// <summary>
        /// Ensure
        /// </summary>
        [TestMethod]
        public void AppStateBase_ToFixedUrl_Root_Slash()
        {
            var state = GetService<AppStateBase>();
            state.ToRelativeUrl("/").Should().Be("");
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_ToFixedUrl_Root_Blank()
        {
            var state = GetService<AppStateBase>();
            state.ToRelativeUrl("").Should().Be("");
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_ToFixedUrl_NotRoot_NoParameter_Slash()
        {
            var state = GetService<AppStateBase>();
            state.ToRelativeUrl("/2").Should().Be("2");
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_ToFixedUrl_NotRoot_NoParameter_NoSlash()
        {
            var state = GetService<AppStateBase>();
            state.ToRelativeUrl("2").Should().Be("2");
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_ToFixedUrl_NotRoot_Parameter_Slash()
        {
            var state = GetService<AppStateBase>();
            state.ToRelativeUrl("/2/Hello").Should().Be("2/Hello");
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_ToFixedUrl_NotRoot_Parameter_NoSlash()
        {
            var state = GetService<AppStateBase>();
            state.ToRelativeUrl("2/Hello").Should().Be("2/Hello");
        }

        #endregion

    }

}
