using CloudNimble.BlazorEssentials.Merlin;
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
    public class OperationTests
    {

        #region Private Members

        private string Echo(string input)
        {
            Thread.Sleep(2000);
            return input;
        }

        #endregion

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void Operation_CorrectInitialState()
        {
            var opTitle = "Test Operation";
            var operation = new Operation(opTitle, null, "Success", "Fail");
            operation.Should().NotBeNull();
            operation.Title.Should().Be(opTitle);
            operation.Steps.Should().NotBeNull();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Success.Should().Be("Success");
            operation.DisplayText.Failure.Should().Be("Fail");
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();
        }

        /// <summary>
        /// Make sure that the step goes through the right transitions on a successful action.
        /// </summary>
        [TestMethod]
        public void Operation_SucceedsCorrectly()
        {
            var step1Complete = false;
            var step2Complete = false;

            var steps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { Thread.Sleep(2000); step1Complete = true; return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { Thread.Sleep(2000); step2Complete = true; return Task.FromResult(true); })
            };
            var operation = new Operation("Test Operation", steps, "Success", "Fail");
            
            operation.Should().NotBeNull();
            operation.Steps.Should().NotBeNullOrEmpty();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();

            // when the operation starts, it will fire off all the operation steps on another thread and return immediately
            operation.Start();

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.IsSubmitting.Should().BeTrue();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            //operation.ProgressPercent.Should().Be(.25M);
            operation.ProgressPercent.Should().Be(0M);

            // this will force the current thread to stop here until the first step indicates completion (includes a 10 second escape hatch)
            SpinWait.SpinUntil(() => { return step1Complete; }, 10000);

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.InProgress);
            //operation.ProgressPercent.Should().Be(.75M);
            operation.ProgressPercent.Should().Be(.5M);

            // this will force the current thread to stop here until the first step indicates completion (includes a 10 second escape hatch)
            SpinWait.SpinUntil(() => { return step2Complete; }, 10000);

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            operation.ProgressPercent.Should().Be(1M);
            operation.CurrentIcon.Should().Be("fa-thumbs-up");
            operation.CurrentIconColor.Should().Be("text-success");
            operation.CurrentProgressClass.Should().Be("bg-success");
            operation.IsSubmitting.Should().BeFalse();
            operation.IsSubmitted.Should().BeTrue();
            operation.Succeeded.Should().BeTrue();
            operation.Steps.All(c => c.Status == OperationStepStatus.Succeeded).Should().BeTrue();
            operation.Steps.Any(c => c.Status == OperationStepStatus.Failed).Should().BeFalse();
        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [TestMethod]
        public void Operation_FailsCorrectly()
        {
            var step1Complete = false;
            var step2Complete = false;

            var steps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { Thread.Sleep(2000); step1Complete = true; return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { Thread.Sleep(2000); step2Complete = true; return Task.FromResult(false); })
            };
            var operation = new Operation("Test Operation", steps, "Success", "Fail");

            operation.Should().NotBeNull();
            operation.Steps.Should().NotBeNullOrEmpty();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();

            // when the operation starts, it will fire off all the operation steps on another thread and return immediately
            operation.Start();

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.IsSubmitting.Should().BeTrue();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            //operation.ProgressPercent.Should().Be(.25M);
            operation.ProgressPercent.Should().Be(0M);

            // this will force the current thread to stop here until the first step indicates completion (includes a 10 second escape hatch)
            SpinWait.SpinUntil(() => { return step1Complete; }, 10000);

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.InProgress);
            //operation.ProgressPercent.Should().Be(.75M);
            operation.ProgressPercent.Should().Be(.5M);

            // this will force the current thread to stop here until the first step indicates completion (includes a 10 second escape hatch)
            SpinWait.SpinUntil(() => { return step2Complete; }, 10000);

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            operation.ProgressPercent.Should().Be(.5M);
            operation.CurrentIcon.Should().Be("fa-thumbs-down");
            operation.CurrentIconColor.Should().Be("text-danger");
            operation.CurrentProgressClass.Should().Be("bg-danger");
            operation.IsSubmitting.Should().BeFalse();
            operation.IsSubmitted.Should().BeTrue();
            operation.Succeeded.Should().BeFalse();
            operation.Steps.All(c => c.Status == OperationStepStatus.Succeeded).Should().BeFalse();
            operation.Steps.Any(c => c.Status == OperationStepStatus.Failed).Should().BeTrue();
        }

        [TestMethod]
        public void Operation_CanChainResults()
        {
            string result = null;

            var steps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { result = Echo("The quick brown fox"); return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { result = Echo($"{result} jumped"); return Task.FromResult(true); }),
                new OperationStep(2, "Step 3", () => { result = Echo($"{result} over the lazy dog."); return Task.FromResult(true); })
            };
            var operation = new Operation("Limerick", steps, "Success", "Fail");

            operation.Should().NotBeNull();
            operation.Steps.Should().NotBeNullOrEmpty();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();

            // when the operation starts, it will fire off all the operation steps on another thread and return immediately
            operation.Start();

            // this will force the current thread to stop here until the operation completes (includes a 10 second escape hatch)
            SpinWait.SpinUntil(() => operation.Succeeded, 10000);

            // give the operation a half-second to update its state
            Thread.Sleep(500);

            operation.CurrentIcon.Should().Be("fa-thumbs-up");
            operation.CurrentIconColor.Should().Be("text-success");
            operation.CurrentProgressClass.Should().Be("bg-success");
            operation.IsSubmitting.Should().BeFalse();
            operation.IsSubmitted.Should().BeTrue();
            operation.Succeeded.Should().BeTrue();
            operation.Steps.All(c => c.Status == OperationStepStatus.Succeeded).Should().BeTrue();
            operation.Steps.Any(c => c.Status == OperationStepStatus.Failed).Should().BeFalse();

            result.Should().Be("The quick brown fox jumped over the lazy dog.");

        }

    }

}
