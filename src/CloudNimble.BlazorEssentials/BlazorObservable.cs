using CloudNimble.BlazorEssentials.Threading;
using CloudNimble.EasyAF.Core;
using System;
using System.Globalization;

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

        #endregion

        #region Properties

        /// <summary>
        /// A <see cref="LoadingStatus"/> specifying the current state of the required data for this ViewModel.
        /// </summary>
        public LoadingStatus LoadingStatus
        {
            get => loadingStatus;
            set => Set(() => LoadingStatus, ref loadingStatus, value);
        }

        public StateHasChangedConfig StateHasChangedConfig { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="BlazorObservable" /> class.
        /// </summary>
        public BlazorObservable(StateHasChangedConfig stateHasChangedConfig = null)
        {
            StateHasChangedConfig = stateHasChangedConfig ?? new();
            StateHasChangedConfig.BlazorObservable = this;
            StateHasChangedConfig.Action = () =>
            {
                if (StateHasChangedConfig.DebugMode != StateHasChangedDebugMode.Off)
                {
                    Console.WriteLine($"WARNING: {GetType().Name} called empty StateHasChangedConfig.Action. Make sure to set `[YourViewModel].StateHasChangedConfig.Action = StateHasChanged;` in OnInitializedAsync()");
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
                    // Dispose here
                }

                disposedValue = true;
                base.Dispose(disposing);
            }
        }

        #endregion

    }

}
