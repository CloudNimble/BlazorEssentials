using CloudNimble.BlazorEssentials.Navigation;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class NavigationItemTests
    {

        #region Private Members

        Func<Task<bool>> trueAction = () => { Thread.Sleep(2000); return Task.FromResult(true); };
        Func<Task<bool>> falseAction = () => { Thread.Sleep(2000); return Task.FromResult(false); };

        #endregion

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void NavigationItem_CorrectInitialState()
        {
            var item1 = new NavigationItem();
            item1.Roles.Should().NotBeNull();
            var item2 = new NavigationItem("Test", "Icon", "");
            item2.Roles.Should().NotBeNull();
            item2.Text.Should().Be("Test");
            item2.Icon.Should().Be("Icon");
            item2.Url.Should().Be("");

        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [TestMethod]
        public void OperationStep_TrueForAnonymous()
        {
            var item = new NavigationItem("", "", "", "", false, "", "", "", true);
            item.IsVisible.Should().Be(false);
            item.IsVisibleToUser(null).Should().BeTrue();
        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [TestMethod]
        public void OperationStep_FalseForAnonymous()
        {
            var item = new NavigationItem("", "", "", "", false, "", "");
            item.IsVisible.Should().Be(false);
            item.IsVisibleToUser(null).Should().BeFalse();
        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [TestMethod]
        public void OperationStep_TrueForEmpty()
        {
            var item = new NavigationItem("", "", "", "", false, "", "");
            item.IsVisible.Should().Be(false);
            var principal = GetEmptyClaimsPrincipal();
            item.IsVisibleToUser(principal).Should().BeTrue();
        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [TestMethod]
        public void OperationStep_FalseForEmpty()
        {
            var item = new NavigationItem("", "", "", "", false, "", "", "admin, test");
            item.IsVisible.Should().Be(false);
            var principal = GetEmptyClaimsPrincipal();
            item.IsVisibleToUser(principal).Should().BeFalse();
        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [TestMethod]
        public void OperationStep_TrueForAdmin()
        {
            var item = new NavigationItem("", "", "", "", false, "", "", "admin, test");
            item.IsVisible.Should().Be(false);
            var principal = GetAdminClaimsPrincipal();
            item.IsVisibleToUser(principal).Should().BeTrue();
        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [TestMethod]
        public void OperationStep_TrueForTest()
        {
            var item = new NavigationItem("", "", "", "", false, "", "", "admin, test");
            item.IsVisible.Should().Be(false);
            var principal = GetTestClaimsPrincipal();
            item.IsVisibleToUser(principal).Should().BeTrue();
        }

        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ClaimsPrincipal GetTestClaimsPrincipal()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Role, "test") }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ClaimsPrincipal GetAdminClaimsPrincipal()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Role, "test") }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ClaimsPrincipal GetEmptyClaimsPrincipal()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()));
        }

        #endregion

    }

}
