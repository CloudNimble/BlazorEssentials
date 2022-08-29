using CloudNimble.BlazorEssentials.Breakdance;
using CloudNimble.EasyAF.Configuration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.BlazorEssentials.Tests.Breakdance
{

    /// <summary>
    /// Tests the functionality of <see cref="BlazorEssentialsTestBase{ConfigurationBase, AppStateBase}"/>.
    /// </summary>
    [TestClass]
    public class BlazorEssentialsTestBase_CoreTests
    {

        /// <summary>
        /// Tests whether or not a <see cref="Bunit.TestContext"/> is created on setup. />
        /// </summary>
        [TestMethod]
        public void BlazorEssentialsTestBase_Constructor_CorrectInitialState()
        {

            var testClass = new BlazorEssentialsTestBase<ConfigurationBase, AppStateBase>();
            testClass.TestHost.Should().BeNull();
            testClass.BUnitTestContext.Should().BeNull();
            testClass.RegisterServices.Should().BeNull();
        }

        /// <summary>
        /// Tests whether or not a <see cref="Bunit.TestContext"/> is created on setup. />
        /// </summary>
        [TestMethod]
        public void BlazorEssentialsTestBase_AssemblySetup_HasCoreServices()
        {

            var testClass = new BlazorEssentialsTestBase<ConfigurationBase, AppStateBase>();
            testClass.TestHost.Should().BeNull();
            testClass.BUnitTestContext.Should().BeNull();
            testClass.RegisterServices.Should().BeNull();

            testClass.AssemblySetup("TestApp");

            testClass.TestHost.Should().NotBeNull();
            testClass.BUnitTestContext.Should().BeNull();
            testClass.RegisterServices.Should().BeNull();

            testClass.TestSetup("TestApp");

            testClass.BUnitTestContext.Should().NotBeNull();
            testClass.RegisterServices.Should().BeNull();

            testClass.GetService<ConfigurationBase>().Should().NotBeNull();
            testClass.GetService<AppStateBase>().Should().NotBeNull();
        }

    }

}
