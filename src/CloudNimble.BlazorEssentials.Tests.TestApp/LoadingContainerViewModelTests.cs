using CloudNimble.BlazorEssentials.Breakdance;
using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.BlazorEssentials.TestApp.ViewModels;
using CloudNimble.EasyAF.Configuration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Tests.TestApp
{

    /// <summary>
    /// Tests the functionality of the <see cref="LoadingContainerViewModel"/>.
    /// </summary>
    /// <remarks>RWM: This is how developers should structure their tests.</remarks>
    [TestClass]
    public class LoadingContainerViewModelTests : BlazorEssentialsTestBase<ConfigurationBase, AppState>
    {

        public LoadingContainerViewModelTests()
        {
            ClassSetup("TestApp");
        }

        #region Test Lifecycle

        [TestInitialize]
        public void Setup()
        {
            TestSetup("TestApp");
            BUnitTestContext.Services.AddSingleton<LoadingContainerViewModel>();
        }

        [TestCleanup]
        public void TearDown() => TestTearDown();

        #endregion

        /// <summary>
        /// Checks to make sure the <see cref="LoadingContainerViewModel" /> is registered with the <see cref="BUnitTestContext"/>.
        /// </summary>
        [TestMethod]
        public void LoadingContainerViewModel_IsRegistered()
        {
            var viewModel = GetService<LoadingContainerViewModel>();
            viewModel.Should().NotBeNull();
        }

        /// <summary>
        /// Triggers <see cref="LoadingContainerViewModel.Load"/> and makes sure the events happen in the proper sequence.
        /// </summary>
        /// <returns>A Task that can be controlled by the unit test runner.</returns>
        [TestMethod]
        public async Task LoadingContainerViewModel_Load_ShouldCycleData()
        {
            var viewModel = GetService<LoadingContainerViewModel>();
            viewModel.Should().NotBeNull();
            viewModel.LoadingStatus.Should().Be(LoadingStatus.NotLoaded);
            viewModel.Item.Should().BeNull();
            viewModel.Items.Should().BeNullOrEmpty();
            viewModel.NoItems.Should().BeNullOrEmpty();

            // https://fluentassertions.com/eventmonitoring/
            using var monitoredSubject = viewModel.Monitor();

            _ = viewModel.Load();
            await Task.Delay(5050);
            viewModel.LoadingStatus.Should().Be(LoadingStatus.Loaded);
            viewModel.Item.Should().NotBeNull();
            viewModel.Items.Should().HaveCount(2).And.NotBeNullOrEmpty();
            viewModel.NoItems.Should().NotBeNull().And.BeEmpty();

            monitoredSubject.Should().RaisePropertyChangeFor(c => viewModel.LoadingStatus);

            await Task.Delay(5050);
            viewModel.LoadingStatus.Should().Be(LoadingStatus.Failed);
            viewModel.Item.Should().NotBeNull();
            viewModel.Items.Should().HaveCount(2).And.NotBeNullOrEmpty();
            viewModel.NoItems.Should().NotBeNull().And.BeEmpty();

            //RWM: We need to be able to check for BOTH PropertyChanged events.
            monitoredSubject.Should().RaisePropertyChangeFor(c => viewModel.LoadingStatus);
        }

    }

}
