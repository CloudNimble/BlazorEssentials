using CloudNimble.BlazorEssentials.Merlin;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class OperationStepTests
    {

        #region Private Members

        Func<Task<bool>> trueAction = () => { Thread.Sleep(2000); return Task.FromResult(true); };
        Func<Task<bool>> falseAction = () => { Thread.Sleep(2000); return Task.FromResult(false); };

        #endregion

        /// <summary>
        /// Make sure that the Step is initialized properly.
        /// </summary>
        [TestMethod]
        public void OperationStep_CorrectInitialState()
        {
            var title = "Test Step";
            var step = new OperationStep(1, title, trueAction);
            step.Should().NotBeNull();
            step.Status.Should().Be(OperationStepStatus.NotStarted);
            step.OnAction.Should().NotBeNull();
            step.DisplayText.Should().Be(title);
            step.ErrorText.Should().BeNullOrWhiteSpace();
        }

        /// <summary>
        /// Make sure that the step goes through the right transitions on a successful action.
        /// </summary>
        [Ignore]
        [TestMethod]
        public void OperationStep_SucceedsCorrectly()
        {
            var title = "Test Step";
            var step = new OperationStep(1, title, trueAction);
            step.Should().NotBeNull();
            step.Start();
            Thread.Sleep(500);
            step.Status.Should().Be(OperationStepStatus.InProgress);
            Thread.Sleep(3000);
            step.Status.Should().Be(OperationStepStatus.Succeeded);
        }

        /// <summary>
        /// Make sure the step goes through the right transitions on a failed action.
        /// </summary>
        [Ignore]
        [TestMethod]
        public void OperationStep_FailsCorrectly()
        {
            var title = "Test Step";
            var step = new OperationStep(1, title, falseAction);
            step.Should().NotBeNull();
            step.Start();
            Thread.Sleep(500);
            step.Status.Should().Be(OperationStepStatus.InProgress);
            Thread.Sleep(3000);
            step.Status.Should().Be(OperationStepStatus.Failed);
        }

    }

}
