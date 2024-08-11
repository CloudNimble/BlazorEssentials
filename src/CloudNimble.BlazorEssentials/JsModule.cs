using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A wrapper that makes it easier to dynamically import JavaScript modules in Blazor. Can be used as the foundation 
    /// to build strongly-typed .NET wrappers around JavaScript libraries.
    /// </summary>
    /// <remarks>
    /// I built this because trying to remember the same pattern for importing JS modules in Blazor was driving me nuts.
    /// </remarks>
    public class JsModule : IJSObjectReference
    {

        #region Private Members

        private readonly IJSRuntime _jsRuntime;
        private readonly string _modulePath;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="JsModule"/> class.
        /// </summary>
        /// <param name="jsRuntime">
        /// The <see cref="IJSRuntime" /> instance that was likely injected by the ViewModel / Page / Control this module is 
        /// being used in.
        /// </param>
        /// <param name="modulePath">
        /// The full path to the JS file to wrap. Should usually be in the format "../content/{packageName}/{pathFromWwwRoot}.js".
        /// </param>
        /// <remarks>
        /// If you don't provide a <paramref name="modulePath"/>, the constructor will attempt to infer it from the calling 
        /// assembly, in the format "../_content/{callingAssemblyName}/{callingAssemblyName}.js".
        /// </remarks>
        public JsModule(IJSRuntime jsRuntime, string modulePath = "")
        {
            _jsRuntime = jsRuntime;
            if (string.IsNullOrWhiteSpace(modulePath))
            {
                var assemblyName = Assembly.GetCallingAssembly().GetName().Name;
                modulePath = $"../_content/{assemblyName}/{assemblyName}.js";
            }
            _modulePath = modulePath;
            Instance = new(() => _jsRuntime.InvokeAsync<IJSObjectReference>("import", _modulePath).AsTask());
        }

        /// <summary>
        /// Creates a new instance of the <see cref="JsModule"/> class.
        /// </summary>
        /// <param name="jsRuntime">
        /// The <see cref="IJSRuntime" /> instance that was likely injected by the ViewModel / Page / Control this module is 
        /// being used in.
        /// </param>
        /// <param name="packageName"></param>
        /// <param name="modulePath">The path to the file, usually from the 'wwwroot' folder in the base of the project.</param>
        /// <remarks>
        /// The SDK-style project system actually does a really good job of knowing when the PackageId is different from the 
        /// AssemblyName, so this constructor may not be necessary. 
        /// </remarks>
        public JsModule(IJSRuntime jsRuntime, string packageName, string modulePath) :
            this(jsRuntime, $"../_content/{packageName}/{modulePath}.js")
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a <see cref="Lazy{T}" /> reference to the <see cref="Task{TResult}" /> of importing the module through
        /// <see cref="IJSRuntime"/>.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// We're using <see cref="Lazy{T}" /> here to ensure that the module is imported exactly once and only when needed.
        /// <see cref="Lazy{T}" /> manages the instance for us
        /// </remarks>
        public Lazy<Task<IJSObjectReference>> Instance { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Disposes of 
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            await (await Instance.Value).DisposeAsync();
            Instance = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object?[]? args = null)
        {
            return await (await Instance.Value).InvokeAsync<TValue>(identifier, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args = null)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await ValueTask.FromCanceled<TValue>(cancellationToken);
            }
            return await (await Instance.Value).InvokeAsync<TValue>(identifier, cancellationToken, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="timeout"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, TimeSpan timeout, object?[]? args = null)
        {
            return await (await Instance.Value).InvokeAsync<TValue>(identifier, timeout, args);
        }

        #endregion

    }

}
