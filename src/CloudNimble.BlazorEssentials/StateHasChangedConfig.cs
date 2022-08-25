using CloudNimble.BlazorEssentials.Threading;
using System;
using System.Globalization;

namespace CloudNimble.BlazorEssentials
{
    public class StateHasChangedConfig : IDisposable
    {

        #region Private Members

        private Action stateHasChangedAction;
        private readonly DelayDispatcher delayDispatcher = new();

        #endregion

        #region Properties

        /// <summary>
        /// Allows the current Blazor container to pass the StateHasChanged action back to the BlazorObservable so ViewModel operations can 
        /// trigger state changes.
        /// </summary>
        /// <remarks>
        /// Will optionally drop intermediate StateHasChanged calls in a rapidly-updating environment, based on <see cref="DelayMode"/> 
        /// and <see cref="DelayInterval"/>.
        /// </remarks>
        public Action Action
        {
            get
            {
                return DelayMode switch
                {
                    StateHasChangedDelayMode.Debounce => () => delayDispatcher.Debounce(DelayInterval, _ => InternalAction()),
                    StateHasChangedDelayMode.Throttle => () => delayDispatcher.Throttle(DelayInterval, _ => InternalAction()),
                    _ => () => InternalAction()
                };
            }
            set
            {
                stateHasChangedAction = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This is public so 
        /// </remarks>
        public int Count { get; set; }

        /// <summary>
        /// Flag for whether or not the render count and helpful debug feedback/warnings should be logged to the <see cref="Console"/>.
        /// Default is <see cref="StateHasChangedDebugMode.Off"/>.
        /// </summary>
        public StateHasChangedDebugMode DebugMode { get; set; } = StateHasChangedDebugMode.Off;

        /// <summary>
        /// An <see cref="int"/> specifying the number of milliseconds this BlazorObservable should wait between 
        /// <see cref="Action"/> calls. Default is 100 miliseconds.
        /// </summary>
        /// <remarks>
        /// <see cref="DelayMode" /> must be set to <see cref="StateHasChangedDelayMode.Debounce" /> or 
        /// <see cref="StateHasChangedDelayMode.Throttle" /> for this setting to take effect.
        /// </remarks>
        public int DelayInterval { get; set; } = 100;

        /// <summary>
        /// A <see cref="DelayMode" /> indicating whether or not this BlazorObservable should reduce the number of times
        /// <see cref="Action" /> should be called in a given <see cref="DelayInterval" />
        /// Default is <see cref="StateHasChangedDelayMode.Off"/>.
        /// </summary>
        public StateHasChangedDelayMode DelayMode { get; set; } = StateHasChangedDelayMode.Off;

        /// <summary>
        /// The <see cref="BlazorObservable"/> <see cref="Type"/> associated with this Configuration instance.
        /// </summary>
        public Type BlazorObservableType { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Copies the values from this <see cref="StateHasChangedConfig" /> instance into a new one.
        /// </summary>
        /// <param name="observable">The <see cref="BlazorObservable"/> instance the new <see cref="StateHasChangedConfig" /> instance will be used for.</param>
        /// <returns>
        /// A new <see cref="StateHasChangedConfig" /> instance with the values populated from this instance.
        /// </returns>
        /// <remarks>
        /// This is required because if we used DI to inject a <see cref="StateHasChangedConfig" /> instance, we wouldn't be able to
        /// have different configurations per <see cref="ViewModelBase{TConfig, TAppState}" />, AND we would end up overwriting the
        /// <see cref="Action">Actions</see> from other Pages when the value was set.
        /// </remarks>
        public StateHasChangedConfig Clone(BlazorObservable observable)
        {
            return new()
            {
                Count = Count,
                DebugMode = DebugMode,
                DelayInterval = DelayInterval,
                DelayMode = DelayMode,
                BlazorObservableType = observable?.GetType()
            };
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// 
        /// </summary>
        internal void LogDelay()
        {
            Console.WriteLine($"{BlazorObservableType.Name}: StateHasChanged #{Count} called @ {DateTime.UtcNow.ToString("hh:mm:ss.fff", CultureInfo.InvariantCulture)} " +
                $"{(DebugMode != StateHasChangedDebugMode.Off ? $"after {delayDispatcher.DelayCount} dropped calls." : "")}");

            if (DebugMode != StateHasChangedDebugMode.Tuning || DelayMode == StateHasChangedDelayMode.Off) return;

            var diffMiliseconds = DateTime.UtcNow.Subtract(delayDispatcher.TimerStarted).TotalMilliseconds;

            // RWM: We're going to use a Tuple switch statement to simplify 
            var entry = (DelayMode, DelayInterval, delayDispatcher.DelayCount, diffMiliseconds) switch
            {
                (StateHasChangedDelayMode.Debounce, _, _, < 50) => $"Performance: Debounce waited {diffMiliseconds}ms between calls. Delay was imperceptible.",

                (StateHasChangedDelayMode.Debounce, _, _, < 2000) => $"Performance: Debounce waited {diffMiliseconds}ms between calls. Delay was noticeable.",

                (StateHasChangedDelayMode.Throttle, < 50, _, _) => $"Performance: Throttle waited {DelayInterval}ms between calls. Delay was imperceptible.",

                (StateHasChangedDelayMode.Throttle, < 2000, < 10, _) => $"Performance: Throttle waited {DelayInterval}ms between calls," +
                    $" but there were fewer than 10 calls dropped. Delay was imperceptible, but consider using Debounce instead.",

                (StateHasChangedDelayMode.Throttle, < 2000, _, _) => $"Performance: Throttle waited {DelayInterval}ms between calls." +
                    $" If your goal is to reduce the number of repaints but fire them consistently, this is the right setting.",

                _ => $"Performance: {DelayMode} waited {(DelayMode == StateHasChangedDelayMode.Debounce ? diffMiliseconds : DelayInterval)}ms " +
                    $"between calls. Delay was unacceptable. Consider adding a visual 'waiting' indicator for the end user."
            };

            //if (string.IsNullOrWhiteSpace(entry)) return;
            Console.WriteLine(entry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// RWM: DO NOT change this method. Doing anything other than returning the StateHasChanged.Action
        /// will cause an infinite loop!
        /// </remarks>
        internal void InternalAction()
        {
            ++Count;

            stateHasChangedAction();

            if (DebugMode == StateHasChangedDebugMode.Off) return;

            LogDelay();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            delayDispatcher.Dispose();
        }

        #endregion

    }
}
