using CloudNimble.EasyAF.Core;
using System;
using System.ComponentModel;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A base class for Blazor ViewModels to implement <see cref="INotifyPropertyChanged" /> and <see cref="IDisposable" />.
    /// </summary>
    public class BlazorObservable : EasyObservableObject
    {

        #region Private Members

        private bool disposedValue;
        private LoadingStatus loadingStatus;

        #endregion

        #region Properties

        /// <summary>
        /// A <see cref="LoadingStatus"/> specifying the current state of the required data for this Observable.
        /// </summary>
        public LoadingStatus LoadingStatus
        {
            get => loadingStatus;
            set => Set(() => LoadingStatus, ref loadingStatus, value);
        }

        /// <summary>
        /// Determines how to trigger StateHasChanged events in a Blazor component.
        /// </summary>
        public StateHasChangedConfig StateHasChanged { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="BlazorObservable" /> class.
        /// </summary>
        public BlazorObservable(StateHasChangedConfig stateHasChangedConfig = null)
        {
            StateHasChanged = stateHasChangedConfig?.Clone(this) ?? new() { BlazorObservableType = GetType() };
            StateHasChanged.Action = () =>
            {
                if (StateHasChanged.DebugMode != StateHasChangedDebugMode.Off)
                {
                    Console.WriteLine($"WARNING: {GetType().Name} called the empty StateHasChanged.Action method. Make sure to set `[YourViewModel].StateHasChanged.Action = StateHasChanged;` in OnInitializedAsync()");
                }
            };
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
                    StateHasChanged?.Dispose();
                }

                disposedValue = true;
                base.Dispose(disposing);
            }
        }

        #endregion

    }

}
