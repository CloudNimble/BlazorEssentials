using CloudNimble.BlazorEssentials.Merlin;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Tests
{

    /// <summary>
    /// Tests the functionality of the <see cref="OperationStep"/> in the operation framework.
    /// </summary>
    [TestClass]
    public class OperationStepTests
    {

        /// <summary>
        /// Make sure that the <see cref="OperationStep"/> is initialized properly.
        /// </summary>
        [TestMethod]
        public void OperationStep_InitialState_HasExpectedValues()
        {
            var title = "Test Step";
            var step = new OperationStep(1, title, () => { return Task.FromResult(true); });
            step.Should().NotBeNull();
            step.Status.Should().Be(OperationStepStatus.NotStarted);
            step.OnAction.Should().NotBeNull();
            step.DisplayText.Should().Be(title);
            step.ErrorText.Should().BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Make sure that the <see cref="OperationStep"/> reaches all of its expected states during a successful run.
        /// </summary>
        [TestMethod]
        public void OperationStep_OnSuccess_HasExpectedValues()
        {
            var canComplete = false;
            var hasStarted = false;

            var title = "Test Step";
            var step = new OperationStep(1, title, () => { hasStarted = true; SpinWait.SpinUntil(() => { return canComplete; }, 10000); return Task.FromResult(true); });

            // check initial state
            step.Should().NotBeNull();
            step.Status.Should().Be(OperationStepStatus.NotStarted);
            step.OnAction.Should().NotBeNull();
            step.DisplayText.Should().Be(title);
            step.ErrorText.Should().BeNullOrWhiteSpace();

            // fire off the operation step on another thread so that we can watch its status here
            Task.Run(() =>
            {
                step.Start();
            }).ConfigureAwait(false);
            SpinWait.SpinUntil(() => { return hasStarted; }, 10000);

            // check for in-progress state
            step.Status.Should().Be(OperationStepStatus.InProgress);
            step.ErrorText.Should().BeNullOrWhiteSpace();

            // allow the step to complete
            canComplete = true;

            // ensure that the step reaches the final state without an error
            var hasCompleted = SpinWait.SpinUntil(() => { return step.Status > OperationStepStatus.InProgress; }, 10000);
            hasCompleted.Should().BeTrue();

            step.Status.Should().Be(OperationStepStatus.Succeeded);
            step.ErrorText.Should().BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Make sure that the <see cref="OperationStep"/> reaches all of its expected states during a failed run.
        /// </summary>
        [TestMethod]
        public void OperationStep_OnFailure_HasExpectedValues()
        {
            var canComplete = false;
            var hasStarted = false;

            var title = "Test Step";
            var step = new OperationStep(1, title, () => { hasStarted = true; SpinWait.SpinUntil(() => { return canComplete; }, 10000); return Task.FromResult(false); });

            // check initial state
            step.Should().NotBeNull();
            step.Status.Should().Be(OperationStepStatus.NotStarted);
            step.OnAction.Should().NotBeNull();
            step.DisplayText.Should().Be(title);
            step.ErrorText.Should().BeNullOrWhiteSpace();

            // fire off the operation step on another thread so that we can watch its status here
            Task.Run(() =>
            {
                step.Start();
            }).ConfigureAwait(false);
            SpinWait.SpinUntil(() => { return hasStarted; }, 10000);

            // check for in-progress state
            step.Status.Should().Be(OperationStepStatus.InProgress);
            step.ErrorText.Should().BeNullOrWhiteSpace();

            // allow the step to complete
            canComplete = true;

            // ensure that the step reaches the final state without an error
            var hasCompleted = SpinWait.SpinUntil(() => { return step.Status > OperationStepStatus.InProgress; }, 10000);
            hasCompleted.Should().BeTrue();

            step.Status.Should().Be(OperationStepStatus.Failed);
            step.ErrorText.Should().BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Make sure that the <see cref="OperationStep"/> reaches all of its expected states after a reset.
        /// </summary>
        [TestMethod]
        public void OperationStep_OnReset_HasExpectedValues()
        {
            var canComplete = false;
            var hasStarted = false;

            var title = "Test Step";
            var step = new OperationStep(1, title, () => { hasStarted = true; SpinWait.SpinUntil(() => { return canComplete; }, 10000); return Task.FromResult(false); });

            // check initial state
            step.Should().NotBeNull();
            step.Status.Should().Be(OperationStepStatus.NotStarted);
            step.OnAction.Should().NotBeNull();
            step.DisplayText.Should().Be(title);
            step.ErrorText.Should().BeNullOrWhiteSpace();

            // fire off the operation step on another thread so that we can watch its status here
            Task.Run(() =>
            {
                step.Start();
            }).ConfigureAwait(false);
            SpinWait.SpinUntil(() => { return hasStarted; }, 10000);

            // check for in-progress state
            step.Status.Should().Be(OperationStepStatus.InProgress);
            step.ErrorText.Should().BeNullOrWhiteSpace();

            // allow the step to complete
            canComplete = true;

            // ensure that the step reaches the final state without an error
            var hasCompleted = SpinWait.SpinUntil(() => { return step.Status > OperationStepStatus.InProgress; }, 10000);
            hasCompleted.Should().BeTrue();

            step.Status.Should().Be(OperationStepStatus.Failed);
            step.ErrorText.Should().BeNullOrWhiteSpace();

            // reset the step and check its state again
            step.ErrorText = "reset should clear this text!";
            step.Reset();
            step.Status.Should().Be(OperationStepStatus.NotStarted);
            step.ErrorText.Should().BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Make sure that the <see cref="OperationStep"/> raises the PropertyChanged event the correct number of times during its lifecycle
        /// </summary>
        [TestMethod]
        public void OperationStep_FullLifecycle_RaisesExpectedEvents()
        {
            var canComplete = false;
            var hasStarted = false;

            var title = "Test Step";
            var step = new OperationStep(1, title, () => { hasStarted = true; SpinWait.SpinUntil(() => { return canComplete; }, 10000); return Task.FromResult(true); });
            using var monitor = step.Monitor();

            // check initial state
            step.Should().NotBeNull();
            step.Status.Should().Be(OperationStepStatus.NotStarted);
            step.OnAction.Should().NotBeNull();
            step.DisplayText.Should().Be(title);
            step.ErrorText.Should().BeNullOrWhiteSpace();

            // fire off the operation step on another thread so that we can watch its status here
            Task.Run(() =>
            {
                step.Start();
            }).ConfigureAwait(false);
            SpinWait.SpinUntil(() => { return hasStarted; }, 10000);

            // check for in-progress state
            step.Status.Should().Be(OperationStepStatus.InProgress);
            step.ErrorText.Should().BeNullOrWhiteSpace();

            monitor.OccurredEvents.Where(c => c.EventName == "PropertyChanged").Count().Should().Be(2);
            monitor.Should().RaisePropertyChangeFor(c => c.Status);
            monitor.Should().RaisePropertyChangeFor(c => c.Label);
            monitor.Clear();

            // allow the step to complete
            canComplete = true;

            // ensure that the step reaches the final state without an error
            var hasCompleted = SpinWait.SpinUntil(() => { return step.Status == OperationStepStatus.Succeeded; }, 10000);
            hasCompleted.Should().BeTrue();
            step.ErrorText.Should().BeNullOrWhiteSpace();

            monitor.OccurredEvents.Where(c => c.EventName == "PropertyChanged").Count().Should().Be(2);
            monitor.Should().RaisePropertyChangeFor(c => c.Status);
            monitor.Should().RaisePropertyChangeFor(c => c.Label);
        }

    }
}
