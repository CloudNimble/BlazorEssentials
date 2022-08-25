using Microsoft.AspNetCore.Components;
using System;
using System.Timers;

namespace CloudNimble.BlazorEssentials.Threading
{

    /// <summary>
    /// Provides methods to reduce the number of events that are fired, usually so that rapid,
    /// imperceptible changes are ignored.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Throttle() ensures that events are throttled by the interval specified.
    ///     Only the last event in the interval sequence of events fires.
    /// </para>
    /// <para>
    ///     Debounce() fires an event only after the specified interval has passed
    ///     in which no other pending event has fired. Only the last event in the
    ///     sequence is fired.
    /// </para>
    /// 
    /// Adapted from https://weblog.west-wind.com/posts/2017/Jul/02/Debouncing-and-Throttling-Dispatcher-Events.
    /// </remarks>
    public class DelayDispatcher : IDisposable
    {

        #region Private Members

        private bool disposedValue;
        private Timer timer;
        private Dispatcher dispatcher = Dispatcher.CreateDefault();
        private Action<object> action;
        private object param;

        #endregion

        #region Public Properties

        /// <summary>
        /// The number of events that have been dropped in a given interval.
        /// </summary>
        /// <remarks>
        /// This value is reset every time the built-in <see cref="Timer"/> elapses.
        /// </remarks>
        public int DelayCount { get; internal set; }

        /// <summary>
        /// The <see cref="DateTime"/> that a new <see cref="Timer"/> was started, in UTC.
        /// </summary>
        public DateTime TimerStarted { get; internal set; }

        #endregion

        #region Public Methods

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
        /// <param name="interval">An <see cref="int"/> specifying the <see cref="Timer"/> duration (in milliseconds).</param>
        /// <param name="action">The <see cref="Action"/> to fire when the <see cref="Timer"/> elapses.</param>
        /// <param name="param">Any optional parameters to pass to the <paramref name="action"/>.</param>
        public void Debounce(int interval, Action<object> action, object param = null)
        {
            DelayCount++;
            // kill pending timer and pending ticks
            if (timer is not null)
            {
                timer.Stop();
            }
            else
            {
                TimerStarted = DateTime.UtcNow;
            }

            // timer is recreated for each event and effectively
            // resets the timeout. Action only fires after timeout has fully
            // elapsed without other events firing during the countdown.

            timer = new Timer(interval);
            timer.Elapsed += (s, e) =>
            {
                if (timer is null) return;

                timer?.Stop();
                timer = null;
                dispatcher.InvokeAsync(() => action.Invoke(param));
                DelayCount = 0;
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
        /// <param name="interval">An <see cref="int"/> specifying the <see cref="Timer"/> duration (in milliseconds).</param>
        /// <param name="action">The <see cref="Action"/> to fire when the <see cref="Timer"/> elapses.</param>
        /// <param name="param">Any optional parameters to pass to the <paramref name="action"/>.</param>
        public void Throttle(int interval, Action<object> action, object param = null)
        {
            DelayCount++;
            // We update the action and param so that it is always the latest action parsed to Throttle that gets invoked.
            this.action = action;
            this.param = param;

            // We only create a new timer if the last one was done.
            if (timer is null)
            {
                timer = new Timer(interval);
                timer.Elapsed += (s, e) =>
                {
                    if (timer is null) return;

                    timer?.Stop();
                    timer = null;
                    dispatcher.InvokeAsync(() => this.action.Invoke(this.param));
                    DelayCount = 0;
                };

                timer.Start();
                TimerStarted = DateTime.UtcNow;
            }
        }

        #endregion

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