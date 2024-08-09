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
        /// 
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="modulePath"></param>
        /// <remarks>
        /// If you don't provide a <paramref name="modulePath"/>, the constructor will attempt to infer it from the calling 
        /// assembly, in the format "../_content/{assemblyName}/{assemblyName}.js"
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="packageName"></param>
        /// <param name="modulePath"></param>
        public JsModule(IJSRuntime jsRuntime, string packageName, string modulePath) :
            this(jsRuntime, $"../_content/{packageName}/{modulePath}.js")
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ValueTask DisposeAsync()
        {
            // RWM: Nothing to dispose since we're capturing lazy references.
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Lazy<Task<IJSObjectReference>> Instance() => 
            new(() => _jsRuntime.InvokeAsync<IJSObjectReference>("import", _modulePath).AsTask());

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object[] args)
        {
            var module = await Instance().Value;
            return await module.InvokeAsync<TValue>(identifier, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="identifier"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object[] args)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await ValueTask.FromCanceled<TValue>(cancellationToken);
            }
            var module = await Instance().Value;
            return await module.InvokeAsync<TValue>(identifier, cancellationToken, args);
        }

        #endregion

    }

}
