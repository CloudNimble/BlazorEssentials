using CloudNimble.BlazorEssentials.Server.Middleware;
using System;

namespace Microsoft.AspNetCore.Builder
{

    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/> to add BlazorEssentials.Server middleware.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {

        /// <summary>
        /// Adds Cross-Origin Isolation middleware to the application pipeline.
        /// This enables SharedArrayBuffer and high-resolution timers required by WASM libraries.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>The application builder for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This middleware adds the following headers to responses:
        /// <list type="bullet">
        /// <item><description><c>Cross-Origin-Opener-Policy: same-origin</c></description></item>
        /// <item><description><c>Cross-Origin-Embedder-Policy: require-corp</c></description></item>
        /// <item><description><c>Cross-Origin-Resource-Policy: same-origin</c></description></item>
        /// </list>
        /// </para>
        /// <para>
        /// <strong>Warning:</strong> This may break third-party scripts and iframes.
        /// Use <see cref="UseCrossOriginIsolation(IApplicationBuilder, Action{CrossOriginIsolationOptions})"/>
        /// to configure exclusions.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// // In Program.cs
        /// var app = builder.Build();
        /// app.UseCrossOriginIsolation();
        /// </code>
        /// </example>
        public static IApplicationBuilder UseCrossOriginIsolation(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CrossOriginIsolationMiddleware>();
        }

        /// <summary>
        /// Adds Cross-Origin Isolation middleware to the application pipeline with custom options.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configureOptions">Action to configure the middleware options.</param>
        /// <returns>The application builder for chaining.</returns>
        /// <example>
        /// <code>
        /// // In Program.cs
        /// var app = builder.Build();
        /// app.UseCrossOriginIsolation(options =>
        /// {
        ///     // Use credentialless instead of require-corp for less strict isolation
        ///     options.CoepPolicy = "credentialless";
        ///
        ///     // Exclude API paths from cross-origin isolation
        ///     options.ExcludePaths.Add("/api/");
        ///
        ///     // Exclude external resource paths
        ///     options.ExcludePaths.Add("/external/");
        /// });
        /// </code>
        /// </example>
        public static IApplicationBuilder UseCrossOriginIsolation(
            this IApplicationBuilder app,
            Action<CrossOriginIsolationOptions> configureOptions)
        {
            var options = new CrossOriginIsolationOptions();
            configureOptions(options);
            return app.UseMiddleware<CrossOriginIsolationMiddleware>(options);
        }

        /// <summary>
        /// Adds Cross-Origin Isolation middleware to the application pipeline with the specified options.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="options">The middleware options.</param>
        /// <returns>The application builder for chaining.</returns>
        public static IApplicationBuilder UseCrossOriginIsolation(
            this IApplicationBuilder app,
            CrossOriginIsolationOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);
            return app.UseMiddleware<CrossOriginIsolationMiddleware>(options);
        }

    }

}
