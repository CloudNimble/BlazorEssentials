using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Server.Middleware
{

    /// <summary>
    /// Middleware that adds Cross-Origin Isolation headers (COOP and COEP) to responses.
    /// These headers are required for features like SharedArrayBuffer and high-resolution timers,
    /// which are needed by WASM libraries like TursoDb that use OPFS (Origin Private File System).
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Warning:</strong> Enabling cross-origin isolation may break third-party scripts,
    /// iframes, and CDN resources that are not configured to work in an isolated context.
    /// Test thoroughly before enabling in production.
    /// </para>
    /// <para>
    /// Headers added:
    /// <list type="bullet">
    /// <item><description><c>Cross-Origin-Opener-Policy: same-origin</c> - Isolates the browsing context</description></item>
    /// <item><description><c>Cross-Origin-Embedder-Policy: require-corp</c> - Requires all resources to explicitly grant permission</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // In Program.cs
    /// app.UseCrossOriginIsolation();
    ///
    /// // Or with options
    /// app.UseCrossOriginIsolation(options =>
    /// {
    ///     options.CoopPolicy = "same-origin";
    ///     options.CoepPolicy = "require-corp";
    ///     options.ExcludePaths.Add("/api/");
    /// });
    /// </code>
    /// </example>
    public class CrossOriginIsolationMiddleware
    {

        #region Private Members

        private readonly RequestDelegate _next;
        private readonly CrossOriginIsolationOptions _options;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossOriginIsolationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="options">Configuration options for cross-origin isolation.</param>
        public CrossOriginIsolationMiddleware(RequestDelegate next, CrossOriginIsolationOptions? options = null)
        {
            _next = next;
            _options = options ?? new CrossOriginIsolationOptions();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Invokes the middleware to add cross-origin isolation headers.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Check if this path should be excluded
            var path = context.Request.Path.Value ?? "";
            foreach (var excludePath in _options.ExcludePaths)
            {
                if (path.StartsWith(excludePath, System.StringComparison.OrdinalIgnoreCase))
                {
                    await _next(context);
                    return;
                }
            }

            // Add Cross-Origin-Opener-Policy header
            if (!context.Response.Headers.ContainsKey("Cross-Origin-Opener-Policy"))
            {
                context.Response.Headers["Cross-Origin-Opener-Policy"] = _options.CoopPolicy;
            }

            // Add Cross-Origin-Embedder-Policy header
            if (!context.Response.Headers.ContainsKey("Cross-Origin-Embedder-Policy"))
            {
                context.Response.Headers["Cross-Origin-Embedder-Policy"] = _options.CoepPolicy;
            }

            // Optionally add Cross-Origin-Resource-Policy header
            if (_options.IncludeCorpHeader && !context.Response.Headers.ContainsKey("Cross-Origin-Resource-Policy"))
            {
                context.Response.Headers["Cross-Origin-Resource-Policy"] = _options.CorpPolicy;
            }

            await _next(context);
        }

        #endregion

    }

}
