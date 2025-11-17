using CloudNimble.BlazorEssentials.Merlin;
using CloudNimble.BlazorEssentials.Tests.Pages;
using CloudNimble.Breakdance.Blazor;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudNimble.BlazorEssentials.Tests.Merlin
{

    /// <summary>
    /// Tests the functionality of the Wizard framework
    /// </summary>
    [TestClass]
    public class WizardTests : BlazorBreakdanceTestBase
    {

        #region Test Lifecycle

        [TestInitialize]
        public void Setup() => TestSetup();

        [TestCleanup]
        public void TearDown() => TestTearDown();

        #endregion

        /// <summary>
        /// Tests that a <see cref="Wizard"/> component starts with the expected state
        /// </summary>
        [TestMethod]
        public void Wizard_InitialState_HasExpectedDefaults()
        {
            var component = BUnitTestContext.Render<Wizard>();
            var wizard = component.Instance;

            wizard.Panes.Should().NotBeNull();
            wizard.Operation.Should().NotBeNull();
            wizard.Operation.Title.Should().NotBeNullOrEmpty();
            wizard.IsBackEnabled.Should().BeFalse();
            wizard.IsNextEnabled.Should().BeTrue();
            wizard.IsNextVisible.Should().BeFalse();
            wizard.IsOperationStartVisible.Should().BeFalse();
            wizard.IsFinishVisible.Should().BeTrue();
        }

        /// <summary>
        /// Tests that the <see cref="Wizard"/> creates a default <see cref="Operation"/> if one is not provided.
        /// </summary>
        [TestMethod]
        public void Wizard_CanCreateDefaultOperation()
        {

            var componentWithOperationParam = BUnitTestContext.Render<Wizard>(parameters => parameters
                .Add(c => c.Operation, new Operation("Operation Provided", null, "Success!", "Failure!"))
            );
            componentWithOperationParam.Instance.Operation.Title.Should().Be("Operation Provided");

            var componentWithNoOperationParam = BUnitTestContext.Render<Wizard>();
            componentWithNoOperationParam.Instance.Operation.Title.Should().Be("Default Operation");

        }

        /// <summary>
        /// Tests that the properties on the <see cref="Wizard"/> update as expected when <see cref="WizardPane"/> navigation occurs
        /// </summary>
        [TestMethod]
        public void Wizard_PaneMovement_HasExpectedProperties()
        {
            var component = BUnitTestContext.Render<WizardRender>();
            var wizard = component.Instance.Wizard;

            wizard.Panes.Should().NotBeEmpty();
            wizard.Operation.Should().NotBeNull();
            wizard.Operation.Title.Should().NotBeNullOrEmpty();

            wizard.IsBackEnabled.Should().BeFalse();
            wizard.IsNextEnabled.Should().BeTrue();
            wizard.IsNextVisible.Should().BeTrue();
            wizard.IsOperationStartVisible.Should().BeFalse();
            wizard.IsFinishVisible.Should().BeFalse();

            // trigger move to next content pane and re-evaluate
            component.Instance.Next();
            wizard.IsBackEnabled.Should().BeTrue();
            wizard.IsNextEnabled.Should().BeTrue();
            wizard.IsNextVisible.Should().BeFalse();
            wizard.IsOperationStartVisible.Should().BeTrue();
            wizard.IsFinishVisible.Should().BeFalse();

            // trigger back and re-evaluate
            component.Instance.Back();
            wizard.IsBackEnabled.Should().BeFalse();
            wizard.IsNextEnabled.Should().BeTrue();
            wizard.IsNextVisible.Should().BeTrue();
            wizard.IsOperationStartVisible.Should().BeFalse();
            wizard.IsFinishVisible.Should().BeFalse();

            // trigger reset and re-evaluate
            component.Instance.Reset();
            wizard.IsBackEnabled.Should().BeFalse();
            wizard.IsNextEnabled.Should().BeTrue();
            wizard.IsNextVisible.Should().BeTrue();
            wizard.IsOperationStartVisible.Should().BeFalse();
            wizard.IsFinishVisible.Should().BeFalse();

            // trigger navigation to final panel (2 steps) and re-evaluate
            component.Instance.Next();
            component.Instance.Next();
            wizard.IsBackEnabled.Should().BeTrue();
            wizard.IsNextEnabled.Should().BeTrue();
            wizard.IsNextVisible.Should().BeFalse();
            wizard.IsOperationStartVisible.Should().BeTrue();
            wizard.IsFinishVisible.Should().BeFalse();
        }

    }

}
