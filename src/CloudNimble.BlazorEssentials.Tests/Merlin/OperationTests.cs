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
            var operation = new Operation(title, operationSteps, "Success", "Fail", "In Progress", "Not Started");
            operation.Should().NotBeNull();

            operation.CurrentIcon.Should().BeEmpty();
            operation.CurrentIconColor.Should().BeEmpty();
            operation.CurrentProgressClass.Should().BeEmpty();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Failure.Should().Be("Fail");
            operation.DisplayText.InProgress.Should().Be("In Progress");
            operation.DisplayText.NotStarted.Should().Be("Not Started");
            operation.DisplayText.Success.Should().Be("Success");
            operation.LoadingStatus.Should().Be(LoadingStatus.NotLoaded);
            operation.ProgressPercent.Should().Be(0M);
            operation.ProgressText.Should().BeNull();
            operation.ResultText.Should().Be(operation.DisplayText.NotStarted);
            operation.ShowPanel.Should().BeFalse();
            operation.Status.Should().Be(OperationStatus.NotStarted);
            operation.Steps.Should().NotBeNull();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();
            operation.Title.Should().Be(title);
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

            var operationSteps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { SpinWait.SpinUntil(() => { return canCompleteStep1; }, 30000); return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { SpinWait.SpinUntil(() => { return canCompleteStep2; }, 30000); return Task.FromResult(true); })
            };
            var operation = new Operation(title, operationSteps, "Success", "Fail", "In Progress", "Not Started");

            // check the initial property state
            operation.CurrentIcon.Should().BeEmpty();
            operation.CurrentIconColor.Should().BeEmpty();
            operation.CurrentProgressClass.Should().BeEmpty();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Failure.Should().Be("Fail");
            operation.DisplayText.InProgress.Should().Be("In Progress");
            operation.DisplayText.NotStarted.Should().Be("Not Started");
            operation.DisplayText.Success.Should().Be("Success");
            operation.LoadingStatus.Should().Be(LoadingStatus.NotLoaded);
            operation.ProgressPercent.Should().Be(0M);
            operation.ProgressText.Should().BeNull();
            operation.ResultText.Should().Be(operation.DisplayText.NotStarted);
            operation.ShowPanel.Should().BeFalse();
            operation.Status.Should().Be(OperationStatus.NotStarted);
            operation.Steps.Should().NotBeNull();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();
            operation.Title.Should().Be(title);

            // start the opreation and wait for it to move to the InProgress state
            operation.Start();

            var hasStartedOperation = SpinWait.SpinUntil(() => { return operation.Status == OperationStatus.InProgress; }, 30000);
            hasStartedOperation.Should().BeTrue();

            // check for in-progress state (step 1)
            Thread.Sleep(500);  // half-second pause for the properties to update before we check them
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ProgressPercent.Should().Be(.25M);
            operation.ProgressText.Should().Be("Step 1");
            operation.ResultText.Should().Be(operation.DisplayText.InProgress);
            operation.Status.Should().Be(OperationStatus.InProgress);
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.NotStarted);

            // allow step 1 to complete
            canCompleteStep1 = true;

            // whait for step 2 to get started
            var hasStep2Started = SpinWait.SpinUntil(() => { return operation.Steps[1].Status > OperationStepStatus.NotStarted; }, 30000);
            hasStep2Started.Should().BeTrue();

            // check for in-progress state (step 2)
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ProgressPercent.Should().Be(.75M);
            operation.ProgressText.Should().Be("Step 2");
            operation.ResultText.Should().Be(operation.DisplayText.InProgress);
            operation.Status.Should().Be(OperationStatus.InProgress);
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.InProgress);

            // allow step 2 to complete
            canCompleteStep2 = true;

            // wait until the operation has completed
            var hasCompletedOperation = SpinWait.SpinUntil(() => { return operation.Status > OperationStatus.InProgress; }, 30000);
            hasCompletedOperation.Should().BeTrue();

            // check the final state of the operation
            operation.Status.Should().Be(OperationStatus.Succeeded);
            Thread.Sleep(500);  // half-second pause for the properties to update before we check them
            operation.CurrentIcon.Should().Be("fa-thumbs-up");
            operation.CurrentIconColor.Should().Be("text-success");
            operation.CurrentProgressClass.Should().Be("bg-success");
            operation.ResultText.Should().Be(operation.DisplayText.Success);
            operation.Steps.All(c => c.Status == OperationStepStatus.Succeeded).Should().BeTrue();
            operation.Steps.Any(c => c.Status == OperationStepStatus.Failed).Should().BeFalse();
            operation.ProgressPercent.Should().Be(1M);
            operation.ProgressText.Should().BeEmpty();
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

            var operationSteps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { SpinWait.SpinUntil(() => { return canCompleteStep1; }, 30000); return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { SpinWait.SpinUntil(() => { return canCompleteStep2; }, 30000); return Task.FromResult(false); })
            };
            var operation = new Operation(title, operationSteps, "Success", "Fail", "In Progress", "Not Started");

            // check the initial property state
            operation.CurrentIcon.Should().BeEmpty();
            operation.CurrentIconColor.Should().BeEmpty();
            operation.CurrentProgressClass.Should().BeEmpty();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Failure.Should().Be("Fail");
            operation.DisplayText.InProgress.Should().Be("In Progress");
            operation.DisplayText.NotStarted.Should().Be("Not Started");
            operation.DisplayText.Success.Should().Be("Success");
            operation.LoadingStatus.Should().Be(LoadingStatus.NotLoaded);
            operation.ProgressPercent.Should().Be(0M);
            operation.ProgressText.Should().BeNull();
            operation.ResultText.Should().Be(operation.DisplayText.NotStarted);
            operation.ShowPanel.Should().BeFalse();
            operation.Status.Should().Be(OperationStatus.NotStarted);
            operation.Steps.Should().NotBeNull();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();
            operation.Title.Should().Be(title);

            // start the opreation and wait for it to move to the InProgress state
            operation.Start();

            var hasStartedOperation = SpinWait.SpinUntil(() => { return operation.Status == OperationStatus.InProgress; }, 30000);
            hasStartedOperation.Should().BeTrue();

            // check for in-progress state (step 1)
            Thread.Sleep(500);  // half-second pause for the properties to update before we check them
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ProgressPercent.Should().Be(.25M);
            operation.ProgressText.Should().Be("Step 1");
            operation.ResultText.Should().Be(operation.DisplayText.InProgress);
            operation.Status.Should().Be(OperationStatus.InProgress);
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.NotStarted);

            // allow step 1 to complete
            canCompleteStep1 = true;

            // whait for step 2 to get started
            var hasStep2Started = SpinWait.SpinUntil(() => { return operation.Steps[1].Status > OperationStepStatus.NotStarted; }, 30000);
            hasStep2Started.Should().BeTrue();

            // check for in-progress state (step 2)
            Thread.Sleep(500);  // half-second pause for the properties to update before we check them
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ProgressPercent.Should().Be(.75M);
            operation.ProgressText.Should().Be("Step 2");
            operation.ResultText.Should().Be(operation.DisplayText.InProgress);
            operation.Status.Should().Be(OperationStatus.InProgress);
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.InProgress);

            // allow step 2 to complete
            canCompleteStep2 = true;

            // wait until the operation has completed
            var hasCompletedOperation = SpinWait.SpinUntil(() => { return operation.Status > OperationStatus.InProgress; }, 30000);
            hasCompletedOperation.Should().BeTrue();

            // check the final state of the operation
            operation.Status.Should().Be(OperationStatus.Failed);
            Thread.Sleep(500);  // half-second pause for the properties to update before we check them
            operation.CurrentIcon.Should().Be("fa-thumbs-down");
            operation.CurrentIconColor.Should().Be("text-danger");
            operation.CurrentProgressClass.Should().Be("bg-danger");
            operation.ResultText.Should().Be(operation.DisplayText.Failure);
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.Failed);
            operation.ProgressPercent.Should().Be(1M);
            operation.ProgressText.Should().BeEmpty();
        }

        /// <summary>
        /// Make sure that the <see cref="Operation"/> stops processing steps after a failure.
        /// </summary>
        [TestMethod]
        public void Operation_OnFailureAtFirstStep_HasExpectedValues()
        {
            var title = "Test Operation";
            var canCompleteStep1 = false;

            var operationSteps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { SpinWait.SpinUntil(() => { return canCompleteStep1; }, 30000); return Task.FromResult(false); }),
                new OperationStep(2, "Step 2", () => { return Task.FromResult(true); })
            };
            var operation = new Operation(title, operationSteps, "Success", "Fail", "In Progress", "Not Started");

            // check the initial property state
            operation.CurrentIcon.Should().BeEmpty();
            operation.CurrentIconColor.Should().BeEmpty();
            operation.CurrentProgressClass.Should().BeEmpty();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Failure.Should().Be("Fail");
            operation.DisplayText.InProgress.Should().Be("In Progress");
            operation.DisplayText.NotStarted.Should().Be("Not Started");
            operation.DisplayText.Success.Should().Be("Success");
            operation.LoadingStatus.Should().Be(LoadingStatus.NotLoaded);
            operation.ProgressPercent.Should().Be(0M);
            operation.ProgressText.Should().BeNull();
            operation.ResultText.Should().Be(operation.DisplayText.NotStarted);
            operation.ShowPanel.Should().BeFalse();
            operation.Status.Should().Be(OperationStatus.NotStarted);
            operation.Steps.Should().NotBeNull();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();
            operation.Title.Should().Be(title);

            // start the opreation and wait for it to move to the InProgress state
            operation.Start();

            var hasStartedOperation = SpinWait.SpinUntil(() => { return operation.Status == OperationStatus.InProgress; }, 30000);
            hasStartedOperation.Should().BeTrue();

            // check for in-progress state (step 1)
            Thread.Sleep(500);  // half-second pause for the properties to update before we check them
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.ProgressPercent.Should().Be(.25M);
            operation.ProgressText.Should().Be("Step 1");
            operation.ResultText.Should().Be(operation.DisplayText.InProgress);
            operation.Status.Should().Be(OperationStatus.InProgress);
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.NotStarted);

            // allow step 1 to complete
            canCompleteStep1 = true;

            // wait until the operation has completed
            var hasCompletedOperation = SpinWait.SpinUntil(() => { return operation.Status > OperationStatus.InProgress; }, 30000);
            hasCompletedOperation.Should().BeTrue();

            // check the final state of the operation
            operation.Status.Should().Be(OperationStatus.Failed);
            Thread.Sleep(500);  // half-second pause for the properties to update before we check them
            operation.CurrentIcon.Should().Be("fa-thumbs-down");
            operation.CurrentIconColor.Should().Be("text-danger");
            operation.CurrentProgressClass.Should().Be("bg-danger");
            operation.ResultText.Should().Be(operation.DisplayText.Failure);
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Failed);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.NotStarted);
            operation.ProgressPercent.Should().Be(1M);
            operation.ProgressText.Should().BeEmpty();
        }

        [TestMethod]
        public void Operation_MultipleSteps_CanChainResults()
        {
            var title = "Limerick";
            string result = null;

            var operationSteps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", async () => { result = await Echo("The quick"); return true; }),
                new OperationStep(2, "Step 2", async () => { result = await Echo($"{result} brown fox"); return true; }),
                new OperationStep(3, "Step 3", async () => { result = await Echo($"{result} jumped"); return true; }),
                new OperationStep(4, "Step 4", async () => { result = await Echo($"{result} over the"); return true; }),
                new OperationStep(5, "Step 5", async () => { result = await Echo($"{result} lazy dog."); return true; }),
            };
            var operation = new Operation(title, operationSteps, "Success", "Fail", "In Progress", "Not Started");

            // check the initial property state
            operation.CurrentIcon.Should().BeEmpty();
            operation.CurrentIconColor.Should().BeEmpty();
            operation.CurrentProgressClass.Should().BeEmpty();
            operation.DisplayIcon.Should().NotBeNull();
            operation.DisplayIconColor.Should().NotBeNull();
            operation.DisplayProgressClass.Should().NotBeNull();
            operation.DisplayText.Should().NotBeNull();
            operation.DisplayText.Failure.Should().Be("Fail");
            operation.DisplayText.InProgress.Should().Be("In Progress");
            operation.DisplayText.NotStarted.Should().Be("Not Started");
            operation.DisplayText.Success.Should().Be("Success");
            operation.LoadingStatus.Should().Be(LoadingStatus.NotLoaded);
            operation.ProgressPercent.Should().Be(0M);
            operation.ProgressText.Should().BeNull();
            operation.ResultText.Should().Be(operation.DisplayText.NotStarted);
            operation.ShowPanel.Should().BeFalse();
            operation.Status.Should().Be(OperationStatus.NotStarted);
            operation.Steps.Should().NotBeNull();
            operation.Steps.All(c => c.Status == OperationStepStatus.NotStarted).Should().BeTrue();
            operation.Title.Should().Be(title);

            // when the operation starts, it will fire off all the operation steps on another thread and return immediately
            operation.Start();

            // wait until the operation has completed
            var hasCompletedOperation = SpinWait.SpinUntil(() => { return operation.Status > OperationStatus.InProgress; }, 30000);
            hasCompletedOperation.Should().BeTrue();

            // check the final state of the operation
            operation.Status.Should().Be(OperationStatus.Succeeded);
            operation.CurrentIcon.Should().Be("fa-thumbs-up");
            operation.CurrentIconColor.Should().Be("text-success");
            operation.CurrentProgressClass.Should().Be("bg-success");
            operation.ResultText.Should().Be(operation.DisplayText.Success);
            operation.Steps.All(c => c.Status == OperationStepStatus.Succeeded).Should().BeTrue();
            operation.Steps.Any(c => c.Status == OperationStepStatus.Failed).Should().BeFalse();
            operation.ProgressPercent.Should().Be(1M);
            operation.ProgressText.Should().BeEmpty();

            result.Should().Be("The quick brown fox jumped over the lazy dog.");

        }

    }

}
