using Humanizer;
using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Navigation
{

    /// <summary>
    /// 
    /// </summary>
    public class NavigationHistory
    {

        #region Private Members

        private readonly IJSRuntime jsRuntime;

        #endregion

        #region Properties

        // RWM: Someday this could work. Today you can't wait for monitors on WASM.
        //public ScrollRestorationType ScrollRestoration
        //{
        //    get => Enum.Parse<ScrollRestorationType>(jsRuntime.InvokeAsync<string>("eval", $"window.history.scrollRestoration").Result.Humanize(LetterCasing.Title));
        //    set => _ = jsRuntime.InvokeVoidAsync("eval", $"window.history.scrollRestoration = '{Enum.GetName(value).ToLower()}'");
        //}

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsRuntime"></param>
        public NavigationHistory(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This asynchronous method goes to the previous page in session history.
        /// </summary>
        /// <returns></returns>
        public ValueTask Back() => jsRuntime.InvokeVoidAsync("window.history.back");

        /// <summary>
        /// Returns an Integer representing the number of elements in the session history, including the currently loaded page.
        /// </summary>
        /// <returns></returns>
        public ValueTask<int> Count() => jsRuntime.InvokeAsync<int>("eval", "window.history.length");

        /// <summary>
        /// Returns an <typeparamref name="T"/> type representing the state at the top of the history stack.
        /// </summary>
        /// <typeparam name="T">The type of the state data</typeparam>
        /// <returns></returns>
        public async ValueTask<T> CurrentState<T>() => JsonSerializer.Deserialize<T>(await jsRuntime.InvokeAsync<string>("eval", "window.history.state"));

        /// <summary>
        /// This asynchronous method goes to the next page in session history.
        /// </summary>
        /// <returns></returns>
        public ValueTask Forward() => jsRuntime.InvokeVoidAsync("window.history.forward");

        /// <summary>
        /// Allows web applications to explicitly get default scroll restoration behavior on history navigation.
        /// </summary>
        /// <returns></returns>
        public async ValueTask<ScrollRestorationType> GetScrollRestoration()
        {
            return Enum.Parse<ScrollRestorationType>((await jsRuntime.InvokeAsync<string>("eval", $"window.history.scrollRestoration").ConfigureAwait(false)).Humanize(LetterCasing.Title));
        }

        /// <summary>
        /// Asynchronously loads a page from the session history, identified by its relative location to the current page.
        /// </summary>
        /// <param name="index">The index to move back or forward</param>
        /// <returns></returns>
        public ValueTask Go(int index) => jsRuntime.InvokeVoidAsync("window.history.go", index);

        /// <summary>
        /// Pushes the given data onto the session history stack.
        /// </summary>
        /// <typeparam name="T">The type of the state data</typeparam>
        /// <param name="state">The state of the data</param>
        /// <param name="url">The url to navigate</param>
        /// <returns></returns>
        public ValueTask PushState<T>(T state, string url)
        {
            return jsRuntime.InvokeVoidAsync("window.history.pushState", JsonSerializer.Serialize(state), string.Empty, url);
        }

        /// <summary>
        /// Updates the most recent entry on the history stack.
        /// </summary>
        /// <typeparam name="T">The type of the state data</typeparam>
        /// <param name="state">The state of the data</param>
        /// <param name="url">The url to navigate</param>
        /// <returns></returns>
        public ValueTask ReplaceState<T>(T state, string url)
        {
            return jsRuntime.InvokeVoidAsync("window.history.replaceState", JsonSerializer.Serialize(state), string.Empty, url);
        }

        /// <summary>
        /// Allows web applications to explicitly set default scroll restoration behavior on history navigation. This property can be either auto or manual.
        /// </summary>
        /// <param name="scrollRestorationType"></param>
        /// <returns></returns>
        public ValueTask SetScrollRestoration(ScrollRestorationType scrollRestorationType)
        {
            return jsRuntime.InvokeVoidAsync("eval", $"window.history.scrollRestoration = '{Enum.GetName(scrollRestorationType).ToLower()}'");
        }

        #endregion

    }

}
