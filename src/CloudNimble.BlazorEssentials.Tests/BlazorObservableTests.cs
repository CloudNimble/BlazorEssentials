using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var blazorObservable = new BlazorObservable();
            var count = 0;
            blazorObservable.StateHasChanged.Action = () => count++;
            blazorObservable.StateHasChanged.DelayMode = StateHasChangedDelayMode.Debounce;
            blazorObservable.StateHasChanged.DelayInterval = 30;

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(10);
                blazorObservable.StateHasChanged.Action();
            }
            await Task.Delay(50);
            count.Should().Be(1);
        }

        [TestMethod]
        public async Task BlazorObservable_Delay_Throttle()
        {
            var blazorObservable = new BlazorObservable();
            var count = 0;
            blazorObservable.StateHasChanged.Action = () => count++;
            blazorObservable.StateHasChanged.DelayMode = StateHasChangedDelayMode.Throttle;
            blazorObservable.StateHasChanged.DelayInterval = 30;

            for (int i = 0; i < 9; i++)
            {
                await Task.Delay(10);
                blazorObservable.StateHasChanged.Action();
            }
            await Task.Delay(20);
            count.Should().Be(3);
        }
    }
}
