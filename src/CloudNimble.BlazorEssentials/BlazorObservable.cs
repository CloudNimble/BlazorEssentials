using CloudNimble.BlazorEssentials.Threading;
using CloudNimble.EasyAF.Core;
using System;

namespace CloudNimble.BlazorEssentials
{
    /// <summary>
    /// A base class for Blazor ViewModels to implement INotifyPropertyChanged and IDisposable.
    /// </summary>
    public class BlazorObservable : EasyObservableObject
    {

        #region Private Members

        private bool disposedValue;
        private LoadingStatus loadingStatus;
        private Action stateHasChangedAction = () => { };
        private DelayDispatcher delayDispatcher = new();
        private int stateHasChangedCount = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Mode for delaying <see cref="StateHasChangedAction"/>.
        /// Default is <see cref="DelayMode.Off"/>.
        /// </summary>
        public DelayMode DelayMode { get; set; } = DelayMode.Off;

        /// <summary>
        /// Interval for <see cref="DelayDispatcher"/> Debounce or Throttle if it is used on <see cref="StateHasChangedAction"/>.
        /// Default is 100 miliseconds.
        /// </summary>
        public int DelayInterval { get; set; } = 100;

        /// <summary>
        /// Flag for whether or not the render count and helpful debug feedback/warnings should be logged to the <see cref="Console"/>.
        /// Default is false.
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        /// A <see cref="LoadingStatus"/> specifying the current state of the required data for this ViewModel.
        /// </summary>
        public LoadingStatus LoadingStatus
        {
            get => loadingStatus;
            set
            {
                if (loadingStatus != value)
                {
                    loadingStatus = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Allows the current Blazor container to pass the StateHasChanged action back to the BlazorObservable so ViewModel operations can trigger state changes.
        /// </summary>
        public Action StateHasChangedAction
        {
            get
            {
                switch (DelayMode)
                {
                    case DelayMode.Debounce:
                        return () => delayDispatcher.Debounce(DelayInterval, _ =>
                        {
                            stateHasChangedAction();
                            if (DebugMode)
                            {
                                Console.WriteLine("{0} StateHasChanged {1} ({2} invocations) @ {3}", GetType().Name, ++stateHasChangedCount, delayDispatcher.DelayCount, DateTime.UtcNow.ToString());
                                var diffMiliseconds = DateTime.UtcNow.Subtract(delayDispatcher.TimerStarted).TotalMilliseconds;
                                switch (diffMiliseconds)
                                {
                                    case < 50:
                                        Console.WriteLine("StateHasChanged was delayed by {0} miliseconds with a Debounce which is not a humanly perceivable delay as it is below 50.", diffMiliseconds);
                                        break;
                                    case > 2000:
                                        Console.WriteLine("StateHasChanged was delayed by {0} miliseconds with a Debounce which will seem like a disruptive delay. Ensure that you have some sort loading message or animation here.", diffMiliseconds);
                                        break;
                                }
                            }
                        }, null);
                    case DelayMode.Throttle:
                        return () => delayDispatcher.Throttle(DelayInterval, _ =>
                        {
                            stateHasChangedAction();
                            if (DebugMode)
                            {
                                Console.WriteLine("{0} StateHasChanged {1} ({2} invocations) @ {3}", GetType().Name, ++stateHasChangedCount, delayDispatcher.DelayCount, DateTime.UtcNow.ToString());
                                switch (DelayInterval)
                                {
                                    case < 50:
                                        Console.WriteLine("You delayed with a Throttle less than 50 miliseconds which is not a humanly perceivable delay.");
                                        break;
                                    case > 1000 when delayDispatcher.DelayCount <= 10:
                                        Console.WriteLine("You delayed with a Throttle larger than 1000 miliseconds with {0} invocations. You might want to use Debounce mode instead with a shorter interval.", delayDispatcher.DelayCount);
                                        break;
                                    case > 2000:
                                        Console.WriteLine("You delayed with a Throttle larger than 2000 miliseconds which will seem like a disruptive delay. Consider using a shorter interval.");
                                        break;
                                }
                            }
                        }, null);
                };
                if (DebugMode)
                {
                    Console.WriteLine("{0} StateHasChanged {1} (1 invocations) @ {2}", GetType().Name, ++stateHasChangedCount, DateTime.UtcNow.ToString());
                }
                return stateHasChangedAction;
            }
            set
            {
                stateHasChangedAction = value;
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    delayDispatcher.Dispose();
                    //PropertyChanged = null;
                }

                disposedValue = true;
                base.Dispose(disposing);
            }
        }

        #endregion

    }

}