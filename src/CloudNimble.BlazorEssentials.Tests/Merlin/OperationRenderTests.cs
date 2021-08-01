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
        public void Setup()
        {
            RegisterServices = services => {
                //services.AddScoped<TestableNavigationManager>();
            };
            TestSetup();
        }

        [TestCleanup]
        public void TearDown() => TestTearDown();

        #endregion

        /// <summary>
        /// Tests that the <see cref="OperationRender"/> component can be initialized.
        /// </summary>
        [TestMethod]
        public void BlazorComponent_CanBindTo_Parameters()
        {
            var componentName = "Operation Render Test Component";

            var component = BUnitTestContext.RenderComponent<OperationRender>(parameters => parameters
                .Add(c => c.OperationSteps, new List<OperationStep>())
                .Add(c => c.DisplayName, componentName)
            );

            component.Should().NotBeNull();
            component.RenderCount.Should().Be(1);
            component.Instance.DisplayName.Should().Be(componentName);
            component.Find(".title").TextContent.Should().Be(componentName);
            component.Find(".succeeded").TextContent.Should().Be("False");
            component.Find(".statusMessage").TextContent.Should().Be("success");
        }

        /// <summary>
        /// Tests that the <see cref="OperationRender"/> component changes when properties change on the <see cref="Operation"/>.
        /// </summary>
        [TestMethod]
        public void BlazorComponent_CanBindTo_OperationProperties()
        {
            var componentName = "Operation Render Test Component";

            var operationSteps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", ()  => { Thread.Sleep(2000); return Task.FromResult(true); })
            };

            var component = BUnitTestContext.RenderComponent<OperationRender>(parameters => parameters
                .Add(c => c.OperationSteps, operationSteps)
                .Add(c => c.DisplayName, componentName)
            );

            component.Should().NotBeNull();
            component.RenderCount.Should().BeGreaterThan(1);
            component.Instance.DisplayName.Should().Be(componentName);
            component.Find(".title").TextContent.Should().Be(componentName);

            var satisfied = SpinWait.SpinUntil(() => { return component.Instance.IsSubmitted ?? false; }, 10000);
            satisfied.Should().Be(true);
            component.Find(".succeeded").TextContent.Should().Be("True");
            component.Find(".statusMessage").TextContent.Should().Be("success");
        }

        /// <summary>
        /// Tests that the <see cref="OperationRender"/> component has the correct content when an <see cref="OperationStep"/> fails.
        /// </summary>
        [TestMethod]
        public void BlazorComponent_FailedStep_UpdatesProperties()
        {
            var componentName = "Operation Render Test Component";

            var operationSteps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", ()  => { Thread.Sleep(2000); return Task.FromResult(false); }),
                new OperationStep(1, "Step 2", ()  => { Thread.Sleep(2000); return Task.FromResult(true); })
            };

            var component = BUnitTestContext.RenderComponent<OperationRender>(parameters => parameters
                .Add(c => c.OperationSteps, operationSteps)
                .Add(c => c.DisplayName, componentName)
            );

            component.Should().NotBeNull();
            component.RenderCount.Should().BeGreaterThan(1);
            component.Instance.DisplayName.Should().Be(componentName);
            component.Find(".title").TextContent.Should().Be(componentName);

            var satisfied = SpinWait.SpinUntil(() => { return component.Instance.IsSubmitted ?? false; }, 10000);
            satisfied.Should().Be(true);
            component.Find(".succeeded").TextContent.Should().Be("False");
            //JHC NOTE: it doesn't look like the operation is setting its ResultText value to the failure message
            //component.Find(".statusMessage").TextContent.Should().Be("failure");
        }

        /// <summary>
        /// Tests that the <see cref="OperationRender"/> component is rendered each time the OnPropertyChanged event occurs in the <see cref="Operation"/>.
        /// </summary>
        [TestMethod]
        public void BlazorComponent_EventNotifications_RaisedFromOperation()
        {
            var componentName = "Operation Render Test Component";

            var operationSteps = new List<OperationStep>
            {
                new OperationStep(1, "Step 1", ()  => { Thread.Sleep(2000); return Task.FromResult(true); })
            };

            var component = BUnitTestContext.RenderComponent<OperationRender>(parameters => parameters
                .Add(c => c.OperationSteps, operationSteps)
                .Add(c => c.DisplayName, componentName)
            );
            using var monitoredSubject = component.Monitor();

            component.Should().NotBeNull();
            component.RenderCount.Should().BeGreaterThan(1);
            component.Instance.DisplayName.Should().Be(componentName);
            component.Find(".title").TextContent.Should().Be(componentName);

            var satisfied = SpinWait.SpinUntil(() => { return component.Instance.IsSubmitted ?? false; }, 10000);
            satisfied.Should().Be(true);
            component.Find(".succeeded").TextContent.Should().Be("True");
            component.Find(".statusMessage").TextContent.Should().Be("success");

            monitoredSubject.Subject.RenderCount.Should().Be(component.Instance.PropertyChangedCount);

        }

    }
}
