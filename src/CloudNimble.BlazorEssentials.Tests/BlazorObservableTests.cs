using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class BlazorObservableTests
    {
        [TestMethod]
        public async Task BlazorObservable_Delay_Off()
        {
            var blazorObservable = new BlazorObservable();
            var count = 0;
            blazorObservable.StateHasChangedConfig.Action = () => count++;
            blazorObservable.StateHasChangedConfig.DelayMode = StateHasChangedDelayMode.Off;

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(20);
                blazorObservable.StateHasChangedConfig.Action();
            }
            count.Should().Be(10);
        }

        [TestMethod]
        public async Task BlazorObservable_Delay_Debounce()
        {
            var blazorObservable = new BlazorObservable();
            var count = 0;
            blazorObservable.StateHasChangedConfig.Action = () => count++;
            blazorObservable.StateHasChangedConfig.DelayMode = StateHasChangedDelayMode.Debounce;
            blazorObservable.StateHasChangedConfig.DelayInterval = 30;

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(10);
                blazorObservable.StateHasChangedConfig.Action();
            }
            await Task.Delay(40);
            count.Should().Be(1);
        }

        [TestMethod]
        public async Task BlazorObservable_Delay_Throttle()
        {
            var blazorObservable = new BlazorObservable();
            var count = 0;
            blazorObservable.StateHasChangedConfig.Action = () => count++;
            blazorObservable.StateHasChangedConfig.DelayMode = StateHasChangedDelayMode.Throttle;
            blazorObservable.StateHasChangedConfig.DelayInterval = 30;

            for (int i = 0; i < 9; i++)
            {
                await Task.Delay(10);
                blazorObservable.StateHasChangedConfig.Action();
            }
            await Task.Delay(20);
            count.Should().Be(3);
        }
    }
}
