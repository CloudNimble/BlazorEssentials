namespace CloudNimble.BlazorEssentials
{
    /// <summary>
    /// Defines how BlazorObservables are monitored to reduce the number of times StateHasChanged is fired.
    /// </summary>
    public enum StateHasChangedDebugMode
    {

        /// <summary>
        /// Does not log StateHasChanged calls to the Browser Console.
        /// </summary>
        Off,

        /// <summary>
        /// Log basic summary information to the Browser Console.
        /// </summary>
        Info,

        /// <summary>
        /// Include performance recommendations in the information logged to the Browser Console.
        /// </summary>
        Tuning,

    }

}
