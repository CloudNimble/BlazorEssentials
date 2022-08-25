using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CloudNimble.BlazorEssentials.Breakdance
{

    /// <summary>
    /// 
    /// </summary>
    public class TestableWebAssemblyHostEnvironment : IWebAssemblyHostEnvironment
    {

        #region Properties

        /// <inheritdoc />
        public string Environment { get; internal set; } = "Development";

        /// <inheritdoc />
        public string BaseAddress { get; internal set; } = "https://localhost";

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public TestableWebAssemblyHostEnvironment()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="baseAddress"></param>
        public TestableWebAssemblyHostEnvironment(string environment, string baseAddress)
        {
        }

        #endregion

    }

}
