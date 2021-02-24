using Microsoft.AspNetCore.Components;
using System;
using System.Timers;

namespace CloudNimble.BlazorEssentials.Threading
{
    /// <summary>
    /// Provides Debounce() and Throttle() methods.
    /// Use these methods to ensure that events aren't handled too frequently.
    /// 
    /// Throttle() ensures that events are throttled by the interval specified.
    /// Only the last event in the interval sequence of events fires.
    /// 
    /// Debounce() fires an event only after the specified interval has passed
    /// in which no other pending event has fired. Only the last event in the
    /// sequence is fired.
    /// </summary>
    /// <remarks>
    /// Adapted from https://weblog.west-wind.com/posts/2017/Jul/02/Debouncing-and-Throttling-Dispatcher-Events.
    /// </remarks>
    public class DelayDispatcher : IDisposable
    {

        #region Private Members

        private bool disposedValue;
        private Timer timer;
        private DateTime timerStarted { get; set; } = DateTime.UtcNow.AddYears(-1);

        #endregion

        /// <summary>
        /// Debounce an event by resetting the event timeout every time the event is 
        /// fired. The behavior is that the Action passed is fired only after events
        /// stop firing for the given timeout period.
        /// 
        /// Use Debounce when you want events to fire only after events stop firing
        /// after the given interval timeout period.
        /// 
        /// Wrap the logic you would normally use in your event code into
        /// the  Action you pass to this method to debounce the event.
        /// Example: https://gist.github.com/RickStrahl/0519b678f3294e27891f4d4f0608519a
        /// </summary>
        /// <param name="interval">Timeout in Milliseconds</param>
        /// <param name="action">Action<object> to fire when debounced event fires</object></param>
        /// <param name="param">optional parameter</param>
        public void Debounce(int interval, Action<object> action, object param = null)
        {
            // kill pending timer and pending ticks
            timer?.Stop();
            timer = null;

            var dispatcher = Dispatcher.CreateDefault();

            // timer is recreated for each event and effectively
            // resets the timeout. Action only fires after timeout has fully
            // elapsed without other events firing in between

            timer = new Timer(interval);
            timer.Elapsed += (s, e) =>
            {
                if (timer == null)
                    return;

                timer?.Stop();
                timer = null;
                dispatcher.InvokeAsync(() => action.Invoke(param));
            };

            timer.Start();
        }

        /// <summary>
        /// This method throttles events by allowing only 1 event to fire for the given
        /// timeout period. Only the last event fired is handled - all others are ignored.
        /// Throttle will fire events every timeout ms even if additional events are pending.
        /// 
        /// Use Throttle where you need to ensure that events fire at given intervals.
        /// </summary>
        /// <param name="interval">Timeout in Milliseconds</param>
        /// <param name="action">Action<object> to fire when debounced event fires</object></param>
        /// <param name="param">optional parameter</param>
        public void Throttle(int interval, Action<object> action, object param = null)
        {
            // kill pending timer and pending ticks
            timer?.Stop();
            timer = null;

            var dispatcher = Dispatcher.CreateDefault();

            var curTime = DateTime.UtcNow;

            // if timeout is not up yet - adjust timeout to fire 
            // with potentially new Action parameters           
            if (curTime.Subtract(timerStarted).TotalMilliseconds < interval)
                interval -= (int)curTime.Subtract(timerStarted).TotalMilliseconds;

            timer = new Timer(interval);
            timer.Elapsed += (s, e) =>
            {
                if (timer == null)
                    return;

                timer?.Stop();
                timer = null;
                dispatcher.InvokeAsync(() => action.Invoke(param));
            };

            timer.Start();
            timerStarted = curTime;
        }

        #region IDisposable

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timer.Dispose();
                    timer = null;
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}