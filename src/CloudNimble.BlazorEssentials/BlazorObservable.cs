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

        #endregion

        #region Properties

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
        public Action StateHasChangedAction { get; set; } = () => { };

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
                    //PropertyChanged = null;
                }

                disposedValue = true;
                base.Dispose(disposing);
            }
        }

        #endregion

    }

}