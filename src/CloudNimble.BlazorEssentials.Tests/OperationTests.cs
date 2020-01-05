using CloudNimble.BlazorEssentials.Merlin;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        //Func<Task<bool>> trueAction = ;
        //Func<Task<bool>> falseAction = () => { Thread.Sleep(2000); return Task.FromResult(false); };

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
        }

        /// <summary>
        /// Make sure that the step goes through the right transitions on a successful action.
        /// </summary>
        [TestMethod]
        public void Operation_SucceedsCorrectly()
        {
            var steps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { Thread.Sleep(2000); return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { Thread.Sleep(2000); return Task.FromResult(true); })
            };
            var operation = new Operation("Test Operation", steps, "Success", "Fail");
            
            operation.Should().NotBeNull();
            operation.Steps.Should().NotBeNullOrEmpty();

            operation.Start();

            Thread.Sleep(500);
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.IsSubmitting.Should().BeTrue();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            operation.ProgressPercent.Should().Be(.25M);
            Thread.Sleep(1700);
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.InProgress);
            operation.ProgressPercent.Should().Be(.75M);
            Thread.Sleep(2000);
            operation.CurrentIcon.Should().Be("fa-thumbs-up");
            operation.CurrentIconColor.Should().Be("text-success");
            operation.CurrentProgressClass.Should().Be("bg-success");
            operation.IsSubmitting.Should().BeFalse();
            operation.IsSubmitted.Should().BeTrue();
            operation.Succeeded.Should().BeTrue();
            operation.Steps.ToList().All(c => c.Status == OperationStepStatus.Succeeded).Should().BeTrue();
            operation.Steps.ToList().Any(c => c.Status == OperationStepStatus.Failed).Should().BeFalse();
        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [TestMethod]
        public void Operation_FailsCorrectly()
        {
            var steps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { Thread.Sleep(2000); return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { Thread.Sleep(2000); return Task.FromResult(false); })
            };
            var operation = new Operation("Test Operation", steps, "Success", "Fail");

            operation.Should().NotBeNull();
            operation.Steps.Should().NotBeNullOrEmpty();

            operation.Start();

            Thread.Sleep(500);
            operation.CurrentIcon.Should().Be("fa-hourglass fa-pulse");
            operation.CurrentIconColor.Should().Be("text-warning");
            operation.CurrentProgressClass.Should().Be("bg-warning");
            operation.IsSubmitting.Should().BeTrue();
            operation.Steps[0].Status.Should().Be(OperationStepStatus.InProgress);
            operation.ProgressPercent.Should().Be(.25M);
            Thread.Sleep(1600);
            operation.Steps[0].Status.Should().Be(OperationStepStatus.Succeeded);
            operation.Steps[1].Status.Should().Be(OperationStepStatus.InProgress);
            operation.ProgressPercent.Should().Be(.75M);
            Thread.Sleep(2000);
            operation.CurrentIcon.Should().Be("fa-thumbs-down");
            operation.CurrentIconColor.Should().Be("text-danger");
            operation.CurrentProgressClass.Should().Be("bg-danger");
            operation.IsSubmitting.Should().BeFalse();
            operation.IsSubmitted.Should().BeTrue();
            operation.Succeeded.Should().BeFalse();
            operation.Steps.ToList().All(c => c.Status == OperationStepStatus.Succeeded).Should().BeFalse();
            operation.Steps.ToList().Any(c => c.Status == OperationStepStatus.Failed).Should().BeTrue();
        }

    }

}
