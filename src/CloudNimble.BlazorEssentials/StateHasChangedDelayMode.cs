namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// Defines how BlazorObservables are monitored to reduce the number of times StateHasChanged is fired.
    /// </summary>
    public enum StateHasChangedDelayMode
    {

        /// <summary>
        /// Does not reduce the number of times StateHasChanged is called.
        /// </summary>
        Off,

        /// <summary>
        /// Only fire StateHasChanged if it hasn't been called in X milliseconds.
        /// </summary>
        Debounce,

        /// <summary>
        /// Only fire StateHasChanged once every X milliseconds.
        /// </summary>
        Throttle,

    }

}
