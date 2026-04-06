using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Tests
{
    /// <summary>
    ///
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class BlazorObservableTests
    {
        /// <summary>
        /// CI runners have unpredictable scheduling latency, so we use wider timing margins there.
        /// </summary>
        private static bool IsCI => Environment.GetEnvironmentVariable("CI") == "true";

        [TestMethod]
        public async Task BlazorObservable_Delay_Off()
        {
            var blazorObservable = new BlazorObservable();
            var count = 0;
            blazorObservable.StateHasChanged.Action = () => count++;
            blazorObservable.StateHasChanged.DelayMode = StateHasChangedDelayMode.Off;

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(20);
                blazorObservable.StateHasChanged.Action();
            }
            count.Should().Be(10);
        }

        [TestMethod]
        public async Task BlazorObservable_Delay_Debounce()
        {
            var debounceInterval = IsCI ? 400 : 200;
            var blazorObservable = new BlazorObservable();
            var count = 0;
            blazorObservable.StateHasChanged.Action = () => count++;
            blazorObservable.StateHasChanged.DelayMode = StateHasChangedDelayMode.Debounce;
            blazorObservable.StateHasChanged.DelayInterval = debounceInterval;

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(30);
                blazorObservable.StateHasChanged.Action();
            }
            await Task.Delay(debounceInterval + 150);
            count.Should().Be(1);
        }

        [TestMethod]
        public async Task BlazorObservable_Delay_Throttle()
        {
            if (IsCI) Assert.Inconclusive("Timing-sensitive test is unreliable on CI runners.");

            var blazorObservable = new BlazorObservable();
            var count = 0;
            blazorObservable.StateHasChanged.Action = () => count++;
            blazorObservable.StateHasChanged.DelayMode = StateHasChangedDelayMode.Throttle;
            blazorObservable.StateHasChanged.DelayInterval = 150;

            for (int i = 0; i < 9; i++)
            {
                await Task.Delay(50);
                blazorObservable.StateHasChanged.Action();
            }
            await Task.Delay(300);
            count.Should().Be(3);
        }
    }
}
