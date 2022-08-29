using CloudNimble.BlazorEssentials.Extensions;
using CloudNimble.BlazorEssentials.Navigation;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CloudNimble.BlazorEssentials.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ListExtensionsTests
    {

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void ListExtensions_Traverse_ReturnsFlatList()
        {
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
            var result = list.Traverse(c => c.Children).ToList();
            result.Should().HaveCount(5);
            result[0].Text.Should().Be("Test1");
            result[1].Text.Should().Be("Inner1");
            result[2].Text.Should().Be("Test2");
            result[3].Text.Should().Be("Inner2");
            result[4].Text.Should().Be("Inner3");
        }

    }

}
