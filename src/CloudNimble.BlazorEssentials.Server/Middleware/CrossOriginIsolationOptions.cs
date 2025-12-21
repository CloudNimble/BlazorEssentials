using System.Collections.Generic;

namespace CloudNimble.BlazorEssentials.Server.Middleware
{

    /// <summary>
    /// Configuration options for the <see cref="CrossOriginIsolationMiddleware"/>.
    /// </summary>
    public class CrossOriginIsolationOptions
    {

        /// <summary>
        /// Gets or sets the Cross-Origin-Opener-Policy value.
        /// Default is "same-origin".
        /// </summary>
        /// <remarks>
        /// Possible values:
        /// <list type="bullet">
        /// <item><description><c>same-origin</c> - Full isolation (required for SharedArrayBuffer)</description></item>
        /// <item><description><c>same-origin-allow-popups</c> - Allows popups to have their own browsing context</description></item>
        /// <item><description><c>unsafe-none</c> - No isolation (default browser behavior)</description></item>
        /// </list>
        /// </remarks>
        public string CoopPolicy { get; set; } = "same-origin";

        /// <summary>
        /// Gets or sets the Cross-Origin-Embedder-Policy value.
        /// Default is "require-corp".
        /// </summary>
        /// <remarks>
        /// Possible values:
        /// <list type="bullet">
        /// <item><description><c>require-corp</c> - Requires all resources to have CORP headers (required for SharedArrayBuffer)</description></item>
        /// <item><description><c>credentialless</c> - Loads cross-origin resources without credentials (less restrictive)</description></item>
        /// <item><description><c>unsafe-none</c> - No embedding restrictions (default browser behavior)</description></item>
        /// </list>
        /// </remarks>
        public string CoepPolicy { get; set; } = "require-corp";

        /// <summary>
        /// Gets or sets whether to include the Cross-Origin-Resource-Policy header.
        /// Default is true.
        /// </summary>
        public bool IncludeCorpHeader { get; set; } = true;

        /// <summary>
        /// Gets or sets the Cross-Origin-Resource-Policy value.
        /// Default is "same-origin".
        /// </summary>
        /// <remarks>
        /// Possible values:
        /// <list type="bullet">
        /// <item><description><c>same-origin</c> - Only same-origin requests can load this resource</description></item>
        /// <item><description><c>same-site</c> - Same-site requests can load this resource</description></item>
        /// <item><description><c>cross-origin</c> - Any origin can load this resource</description></item>
        /// </list>
        /// </remarks>
        public string CorpPolicy { get; set; } = "same-origin";

        /// <summary>
        /// Gets or sets a list of path prefixes to exclude from cross-origin isolation.
        /// Paths starting with these prefixes will not have the headers added.
        /// </summary>
        /// <example>
        /// <code>
        /// options.ExcludePaths.Add("/api/");
        /// options.ExcludePaths.Add("/external/");
        /// </code>
        /// </example>
        public List<string> ExcludePaths { get; set; } = [];

    }

}
