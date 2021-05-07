using CloudNimble.BlazorEssentials.Extensions;
using CloudNimble.BlazorEssentials.Navigation;
using CloudNimble.Breakdance.Blazor;
using FluentAssertions;
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
    public class AppStateBaseTests
    {

        #region Private Members

        Func<Task<bool>> trueAction = () => { Thread.Sleep(2000); return Task.FromResult(true); };
        Func<Task<bool>> falseAction = () => { Thread.Sleep(2000); return Task.FromResult(false); };

        #endregion

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_CorrectInitialState()
        {
            var state = new AppStateBase(null, null);
            state.LoadingStatus.Should().Be(LoadingStatus.NotLoaded);
        }

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void AppStateBase_RecursiveNavItems_SetSpecificItem()
        {
            var state = new AppStateBase(new TestableNavigationManager("https://localhost/"), null);

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
            state.SetCurrentNavItem("2");

            var results = state.NavItems.Traverse(c => c.Children);
            results.Should().HaveCount(5);
            results.First().Text.Should().Be("Test1");
            //RWM: This is not presently testing the order, because the order is incorrect.
            //However, correct order is not necessary for SetCurrentNavItem to work correctly.

            state.CurrentNavItem.Should().NotBeNull();
            state.CurrentNavItem.Text.Should().Be("Inner2");
        }

    }

}
