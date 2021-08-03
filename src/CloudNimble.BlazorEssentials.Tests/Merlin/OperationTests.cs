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

        /// <summary>
        /// Simulates an asynchronous call
        /// </summary>
        /// <param name="input">String to write back to the caller.</param>
        /// <returns></returns>
        private async Task<string> Echo(string input)
        {
            Thread.Sleep(1000);
            return await Task.FromResult(input);
        }

        #endregion

        /// <summary>
        /// Make sure that the <see cref="Operation"/> is initialized properly.
        /// </summary>
        [TestMethod]
        public void Operation_InitialState_HasExpectedValues()
        {
            var title = "Test Operation";
            var operationSteps = new List<OperationStep> { new OperationStep(1, "Step1", () => { return Task.FromResult(true); }) };
            var operation = new Operation(title, operationSteps, "Success", "Fail");
            operation.Should().NotBeNull();
            operation.Title.Should().Be(title);
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
        /// Make sure that the <see cref="Operation"/> reaches all of its expected states during a successful run.
        /// </summary>
        [TestMethod]
        public void Operation_OnSuccess_HasExpectedValues()
        {
            var title = "Test Operation";
            var canCompleteStep1 = false;
            var canCompleteStep2 = false;
            var step1Started = false;
            var step2Started = false;

            var steps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { step1Started = true; SpinWait.SpinUntil(() => { return canCompleteStep1; }, 30000); return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { step2Started = true; SpinWait.SpinUntil(() => { return canCompleteStep2; }, 30000); return Task.FromResult(true); })
            };
            var operation = new Operation(title, steps, "Success", "Fail");

            // check the initial property state
            operation.Should().NotBeNull();
            operation.Title.Should().Be(title);
            operation.Steps.Should().NotBeNull();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Success.Should().Be("Success");
            operation.DisplayText.Failure.Should().Be("Fail");

            // check the state of the properties that update during the operation lifecycle
            operation.CurrentIcon.Should().BeNull();
            operation.CurrentIconColor.Should().BeNull();
            operation.CurrentProgressClass.Should().BeNull();
            operation.ResultText.Should().BeNull();
            operation.IsSubmitting.Should().BeFalse();
            operation.IsSubmitted.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();

            // when the operation starts, it will fire off all the operation steps on another thread and return immediately
            operation.Start();
            SpinWait.SpinUntil(() => { return step1Started; }, 10000);

            // check for in-progress state
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ResultText.Should().BeNull();
            operation.IsSubmitting.Should().BeTrue();
            operation.IsSubmitted.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.NotStarted);
            operation.ProgressPercent.Should().Be(.25M);

            // allow step 1 to complete
            canCompleteStep1 = true;

            // ensure that the first step reaches the final state without an error
            var hasStartedStep2 = SpinWait.SpinUntil(() => { return step2Started; }, 10000);
            hasStartedStep2.Should().BeTrue();

            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ResultText.Should().BeNull();
            operation.IsSubmitting.Should().BeTrue();
            operation.IsSubmitted.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.InProgress);
            operation.ProgressPercent.Should().Be(.75M);

            // allow step 2 to complete
            canCompleteStep2 = true;

            // wait until the operation has completed
            var hasCompletedOperation = SpinWait.SpinUntil(() => { return operation.IsSubmitted; }, 10000);
            hasCompletedOperation.Should().BeTrue();

            // check the final state of the operation
            operation.CurrentIcon.Should().Be("fa-thumbs-up");
            operation.CurrentIconColor.Should().Be("text-success");
            operation.CurrentProgressClass.Should().Be("bg-success");
            operation.ResultText.Should().Be(operation.DisplayText.Success);
            operation.IsSubmitting.Should().BeFalse();
            operation.Succeeded.Should().BeTrue();
            operation.Steps.All(c => c.Status == OperationStepStatus.Succeeded).Should().BeTrue();
            operation.Steps.Any(c => c.Status == OperationStepStatus.Failed).Should().BeFalse();
            operation.ProgressPercent.Should().Be(1M);
        }

        /// <summary>
        /// Make sure that the <see cref="Operation"/> reaches all of its expected states during a failed run.
        /// </summary>
        [TestMethod]
        public void Operation_OnFailure_HasExpectedValues()
        {
            var title = "Test Operation";
            var canCompleteStep1 = false;
            var canCompleteStep2 = false;
            var step1Started = false;
            var step2Started = false;

            var steps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { step1Started = true; SpinWait.SpinUntil(() => { return canCompleteStep1; }, 30000); return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { step2Started = true; SpinWait.SpinUntil(() => { return canCompleteStep2; }, 30000); return Task.FromResult(false); })
            };
            var operation = new Operation(title, steps, "Success", "Fail");

            // check the initial property state
            operation.Should().NotBeNull();
            operation.Title.Should().Be(title);
            operation.Steps.Should().NotBeNull();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Success.Should().Be("Success");
            operation.DisplayText.Failure.Should().Be("Fail");

            // check the state of the properties that update during the operation lifecycle
            operation.CurrentIcon.Should().BeNull();
            operation.CurrentIconColor.Should().BeNull();
            operation.CurrentProgressClass.Should().BeNull();
            operation.ResultText.Should().BeNull();
            operation.IsSubmitting.Should().BeFalse();
            operation.IsSubmitted.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();

            // when the operation starts, it will fire off all the operation steps on another thread and return immediately
            operation.Start();
            SpinWait.SpinUntil(() => { return step1Started; }, 10000);

            // check for in-progress state
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ResultText.Should().BeNull();
            operation.IsSubmitting.Should().BeTrue();
            operation.IsSubmitted.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.NotStarted);
            operation.ProgressPercent.Should().Be(.25M);

            // allow step 1 to complete
            canCompleteStep1 = true;

            // ensure that the first step reaches the final state without an error
            var hasStartedStep2 = SpinWait.SpinUntil(() => { return step2Started; }, 10000);
            hasStartedStep2.Should().BeTrue();

            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ResultText.Should().BeNull();
            operation.IsSubmitting.Should().BeTrue();
            operation.IsSubmitted.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.InProgress);
            operation.ProgressPercent.Should().Be(.75M);

            // allow step 2 to complete
            canCompleteStep2 = true;

            // wait until the operation has completed
            var hasCompletedOperation = SpinWait.SpinUntil(() => { return operation.IsSubmitted; }, 10000);
            hasCompletedOperation.Should().BeTrue();

            // check the final state of the operation
            operation.CurrentIcon.Should().Be("fa-thumbs-down");
            operation.CurrentIconColor.Should().Be("text-danger");
            operation.CurrentProgressClass.Should().Be("bg-danger");
            operation.ResultText.Should().Be(operation.DisplayText.Failure);
            operation.IsSubmitting.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.Failed);
            operation.ProgressPercent.Should().Be(1M);
        }

        /// <summary>
        /// Make sure that the <see cref="Operation"/> stops processing steps after a failure.
        /// </summary>
        [TestMethod]
        public void Operation_OnFailureAtFirstStep_HasExpectedValues()
        {
            var title = "Test Operation";
            var canCompleteStep1 = false;
            var step1Started = false;

            var steps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { step1Started = true; SpinWait.SpinUntil(() => { return canCompleteStep1; }, 30000); return Task.FromResult(false); }),
                new OperationStep(2, "Step 2", () => { return Task.FromResult(true); })
            };
            var operation = new Operation(title, steps, "Success", "Fail");

            // check the initial property state
            operation.Should().NotBeNull();
            operation.Title.Should().Be(title);
            operation.Steps.Should().NotBeNull();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Success.Should().Be("Success");
            operation.DisplayText.Failure.Should().Be("Fail");

            // check the state of the properties that update during the operation lifecycle
            operation.CurrentIcon.Should().BeNull();
            operation.CurrentIconColor.Should().BeNull();
            operation.CurrentProgressClass.Should().BeNull();
            operation.ResultText.Should().BeNull();
            operation.IsSubmitting.Should().BeFalse();
            operation.IsSubmitted.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();

            // when the operation starts, it will fire off all the operation steps on another thread and return immediately
            operation.Start();
            SpinWait.SpinUntil(() => { return step1Started; }, 10000);

            // check for in-progress state
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ResultText.Should().BeNull();
            operation.IsSubmitting.Should().BeTrue();
            operation.IsSubmitted.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.NotStarted);
            operation.ProgressPercent.Should().Be(.25M);

            // allow step 1 to complete
            canCompleteStep1 = true;

            // wait until the operation has completed
            var hasCompletedOperation = SpinWait.SpinUntil(() => { return operation.IsSubmitted; }, 10000);
            hasCompletedOperation.Should().BeTrue();

            // check the final state of the operation
            operation.CurrentIcon.Should().Be("fa-thumbs-down");
            operation.CurrentIconColor.Should().Be("text-danger");
            operation.CurrentProgressClass.Should().Be("bg-danger");
            //operation.ResultText.Should().Be(operation.DisplayText.Failure);
            operation.IsSubmitting.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Failed);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.NotStarted);
            operation.ProgressPercent.Should().Be(1M);
        }

        [TestMethod]
        public void Operation_MultipleSteps_CanChainResults()
        {
            var title = "Limerick";
            string result = null;

            var steps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", async () => { result = await Echo("The quick"); return true; }),
                new OperationStep(2, "Step 2", async () => { result = await Echo($"{result} brown fox"); return true; }),
                new OperationStep(3, "Step 3", async () => { result = await Echo($"{result} jumped"); return true; }),
                new OperationStep(4, "Step 4", async () => { result = await Echo($"{result} over the"); return true; }),
                new OperationStep(5, "Step 5", async () => { result = await Echo($"{result} lazy dog."); return true; }),
            };
            var operation = new Operation(title, steps, "Success", "Fail");

            // check the initial property state
            operation.Should().NotBeNull();
            operation.Title.Should().Be(title);
            operation.Steps.Should().NotBeNull();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Success.Should().Be("Success");
            operation.DisplayText.Failure.Should().Be("Fail");

            // check the state of the properties that update during the operation lifecycle
            operation.CurrentIcon.Should().BeNull();
            operation.CurrentIconColor.Should().BeNull();
            operation.CurrentProgressClass.Should().BeNull();
            operation.ResultText.Should().BeNull();
            operation.IsSubmitting.Should().BeFalse();
            operation.IsSubmitted.Should().BeFalse();
            operation.Succeeded.Should().BeFalse();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();

            // when the operation starts, it will fire off all the operation steps on another thread and return immediately
            operation.Start();

            // wait until the operation has completed
            var hasCompletedOperation = SpinWait.SpinUntil(() => { return operation.IsSubmitted; }, 30000);
            hasCompletedOperation.Should().BeTrue();

            // check the final state of the operation
            operation.CurrentIcon.Should().Be("fa-thumbs-up");
            operation.CurrentIconColor.Should().Be("text-success");
            operation.CurrentProgressClass.Should().Be("bg-success");
            operation.ResultText.Should().Be(operation.DisplayText.Success);
            operation.IsSubmitting.Should().BeFalse();
            operation.Succeeded.Should().BeTrue();
            operation.Steps.All(c => c.Status == OperationStepStatus.Succeeded).Should().BeTrue();
            operation.Steps.Any(c => c.Status == OperationStepStatus.Failed).Should().BeFalse();
            operation.ProgressPercent.Should().Be(1M);

            result.Should().Be("The quick brown fox jumped over the lazy dog.");

        }

    }

}
