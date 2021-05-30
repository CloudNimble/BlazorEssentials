using CloudNimble.BlazorEssentials;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// 
    /// </summary>
    public static class IHttpClientBuilderExtensions
    {

        /// <summary>
        /// Given the <see cref="HttpHandlerMode"/>, adds the specified <typeparamref name="THandler"/> to the beginning or end of the pipeline.
        /// </summary>
        /// <typeparam name="THandler">The <see cref="DelegatingHandler"/> type to pull from the scoped <see cref="ServiceProvider"/>.</typeparam>
        /// <param name="builder">The <see cref="IHttpClientBuilder"/> instance to extend.</param>
        /// <param name="mode">A <see cref="HttpHandlerMode"/> specifying whether we are making this handler the first one in the pipeline, or the last.</param>
        /// <returns></returns>
        public static IHttpClientBuilder AddHttpMessageHandler<THandler>(this IHttpClientBuilder builder, HttpHandlerMode mode)
            where THandler : DelegatingHandler

        {
            return mode == HttpHandlerMode.Add ? builder.AddHttpMessageHandler<THandler>() : builder.ConfigurePrimaryHttpMessageHandler<THandler>();
        }

    }

}
