using CloudNimble.BlazorEssentials.Merlin;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void OperationStep_CorrectInitialState()
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
        /// Make sure that the step goes through the right transitions on a successful action.
        /// </summary>
        [TestMethod]
        public void OperationStep_SucceedsCorrectly()
        {
            var title = "Test Step";
            var actionComplete = false;
            var step = new OperationStep(1, title, () => { Thread.Sleep(2000); actionComplete = true; return Task.FromResult(true); });
            step.Should().NotBeNull();
            step.Status.Should().Be(OperationStepStatus.NotStarted);

            // fire off the operation step on another thread so that we can watch its status here
            Task.Run(() =>
            {
                step.Start();
            }).ConfigureAwait(false);

            // give the operationstep a half-second to spin up
            Thread.Sleep(500);

            step.Status.Should().Be(OperationStepStatus.InProgress);

            // wait until the static flag lets us know that the operation step has finished (includes a 10 second escape hatch)
            SpinWait.SpinUntil(() => { return actionComplete; }, 10000);

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            step.Status.Should().Be(OperationStepStatus.Succeeded);
        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [TestMethod]
        public void OperationStep_FailsCorrectly()
        {
            var title = "Test Step";
            var actionComplete = false;
            var step = new OperationStep(1, title, () => { Thread.Sleep(2000); actionComplete = true; return Task.FromResult(false); });
            step.Should().NotBeNull();
            step.Status.Should().Be(OperationStepStatus.NotStarted);

            // fire off the operation step on another thread so that we can watch its status here
            Task.Run(() =>
            {
                step.Start();
            }).ConfigureAwait(false);

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            step.Status.Should().Be(OperationStepStatus.InProgress);

            // wait until the static flag lets us know that the operation step has finished (includes a 10 second escape hatch)
            SpinWait.SpinUntil(() => { return actionComplete; }, 10000);

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            step.Status.Should().Be(OperationStepStatus.Failed);
        }

    }

}
