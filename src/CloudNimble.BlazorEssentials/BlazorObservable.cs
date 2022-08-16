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
        /// Interval for <see cref="DelayDispatcher"/> Throttle on <see cref="StateHasChangedAction"/>.
        /// Default is 100 miliseconds.
        /// </summary>
        public int ThrottleInterval { get; set; } = 100;


        /// <summary>
        /// Flag for whether or not the render count should be logged to the <see cref="Console"/>.
        /// Default is false.
        /// </summary>
        public bool EnableRenderCount { get; set; }

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
                return () => delayDispatcher.Throttle(ThrottleInterval, _ =>
                {
                    stateHasChangedAction();
                    if (EnableRenderCount)
                    {
                        Console.WriteLine("{0} render {1}", GetType().Name, ++stateHasChangedCount);
                    }
                }, null);
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