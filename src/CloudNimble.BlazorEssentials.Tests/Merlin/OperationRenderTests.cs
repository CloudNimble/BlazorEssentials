using Bunit;
using CloudNimble.BlazorEssentials.Merlin;
using CloudNimble.Breakdance.Blazor;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Tests.Pages
{

    /// <summary>
    /// Tests integrating the Merlin <see cref="Operation"/> framework into a blazor component.
    /// </summary>
    [TestClass]
    public class OperationRenderTests : BlazorBreakdanceTestBase
    {

        #region Test Lifecycle

        [TestInitialize]
        public void Setup() => TestSetup();

        [TestCleanup]
        public void TearDown() => TestTearDown();

        #endregion

        /// <summary>
        /// Tests that the <see cref="OperationRender"/> component can be initialized.
        /// </summary>
        [TestMethod]
        public void RenderComponent_InitialState_HasExpectedValues()
        {
            var title = "Operation Render Test Component";
            var operationSteps = new List<OperationStep> { new OperationStep(1, "Step1", () => { return Task.FromResult(true); }) };

            var component = BUnitTestContext.RenderComponent<OperationRender>(parameters => parameters
                .Add(c => c.OperationSteps, operationSteps)
                .Add(c => c.DisplayName, title)
            );

            // check the initial component state
            component.Should().NotBeNull();
            component.RenderCount.Should().Be(1);
            component.Instance.Should().NotBeNull();
            component.Instance.Status.Should().Be(OperationStatus.NotStarted);

            component.Find("icon").GetAttribute("color").Should().BeEmpty();
            component.Find("icon").GetAttribute("value").Should().BeEmpty();
            component.Find(".operationStatus").TextContent.Should().Be(OperationStatus.NotStarted.ToString());
            component.Find("progress").GetAttribute("class").Should().BeEmpty();
            component.Find("progress").GetAttribute("displayText").Should().BeNull();
            component.Find("progress").GetAttribute("value").As<decimal>().Should().Be(0M);
            component.Find(".propertyChanges").TextContent.As<int>().Should().Be(0);
            component.Find(".resultText").TextContent.Should().Be(component.Instance.DisplayText.NotStarted);
            component.Find("title").TextContent.Should().Be(title);
            component.Find("ul").Children.Count().Should().Be(1);
            component.Find("ul").Children.All(c => c.TextContent.Contains(OperationStatus.NotStarted.ToString())).Should().BeTrue();
        }

        /// <summary>
        /// Tests that the <see cref="OperationRender"/> component changes when properties change on the <see cref="Operation"/>.
        /// </summary>
        [TestMethod]
        public void Operation_OnSuccess_HasExpectedValues()
        {
            var title = "Operation Render Test Component";
            var canCompleteStep1 = false;
            var canCompleteStep2 = false;

            var operationSteps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", () => { SpinWait.SpinUntil(() => { return canCompleteStep1; }, 30000); return Task.FromResult(true); }),
                new OperationStep(2, "Step 2", () => { SpinWait.SpinUntil(() => { return canCompleteStep2; }, 30000); return Task.FromResult(true); })
            };

            var component = BUnitTestContext.RenderComponent<OperationRender>(parameters => parameters
                .Add(c => c.OperationSteps, operationSteps)
                .Add(c => c.DisplayName, title)
            );

            // check the initial component state
            component.Should().NotBeNull();
            component.RenderCount.Should().Be(1);
            component.Instance.Should().NotBeNull();
            component.Instance.Status.Should().Be(OperationStatus.NotStarted);

            component.Find("icon").GetAttribute("color").Should().BeEmpty();
            component.Find("icon").GetAttribute("value").Should().BeEmpty();
            component.Find(".operationStatus").TextContent.Should().Be(OperationStatus.NotStarted.ToString());
            component.Find("progress").GetAttribute("class").Should().BeEmpty();
            component.Find("progress").GetAttribute("displayText").Should().BeNull();
            component.Find("progress").GetAttribute("value").As<decimal>().Should().Be(0M);
            component.Find(".propertyChanges").TextContent.As<int>().Should().Be(0);
            component.Find(".resultText").TextContent.Should().Be(component.Instance.DisplayText.NotStarted);
            component.Find("title").TextContent.Should().Be(title);
            component.Find("ul").Children.Count().Should().Be(2);
            component.Find("ul").Children.All(c => c.TextContent.Contains(OperationStatus.NotStarted.ToString())).Should().BeTrue();

            // start the operation
            component.Instance.Start();

            var hasStartedOperation = SpinWait.SpinUntil(() => { return component.Instance.Status == OperationStatus.InProgress; }, 30000);
            hasStartedOperation.Should().BeTrue();

            // check for in-progress state (step 1)
            Thread.Sleep(500);  // half-second pause for the properties to update before we check them
            component.Find("icon").GetAttribute("color").Should().Be("text-warning");
            component.Find("icon").GetAttribute("value").Should().Be("fa-hourglass fa-pulse");
            component.Find(".operationStatus").TextContent.Should().Be(OperationStatus.InProgress.ToString());
            component.Find("progress").GetAttribute("class").Should().Be("bg-warning");
            component.Find("progress").GetAttribute("displayText").Should().Be("Step 1");
            component.Find("progress").GetAttribute("value").Should().Be("0.25");
            component.Find(".propertyChanges").TextContent.Should().Be($"{component.RenderCount - 1}");
            component.Find(".resultText").TextContent.Should().Be(component.Instance.DisplayText.InProgress);
            component.Find("ul").Children.FirstOrDefault(c => c.GetAttribute("id") == "step_1").TextContent.Contains(OperationStepStatus.InProgress.ToString());
            component.Find("ul").Children.FirstOrDefault(c => c.GetAttribute("id") == "step_2").TextContent.Contains(OperationStepStatus.NotStarted.ToString());

            // allow step 1 to complete
            canCompleteStep1 = true;
            canCompleteStep2 = true;

            // wait until the operation has completed
            var hasCompletedOperation = SpinWait.SpinUntil(() => { return component.Instance.Status > OperationStatus.InProgress; }, 30000);
            hasCompletedOperation.Should().BeTrue();

            // check the final state of the operation
            Thread.Sleep(500);  // half-second pause for the properties to update before we check them
            component.Find("icon").GetAttribute("color").Should().Be("text-success");
            component.Find("icon").GetAttribute("value").Should().Be("fa-thumbs-up");
            component.Find(".operationStatus").TextContent.Should().Be(OperationStatus.Succeeded.ToString());
            component.Find("progress").GetAttribute("class").Should().Be("bg-success");
            component.Find("progress").GetAttribute("displayText").Should().BeEmpty();
            component.Find("progress").GetAttribute("value").Should().Be("1");
            component.Find(".propertyChanges").TextContent.Should().Be($"{component.RenderCount - 1}");
            component.Find(".resultText").TextContent.Should().Be(component.Instance.DisplayText.Success);
            component.Find("ul").Children.All(c => c.TextContent.Contains(OperationStepStatus.Succeeded.ToString())).Should().BeTrue();
        }

    }
}
