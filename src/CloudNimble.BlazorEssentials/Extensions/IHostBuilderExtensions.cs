using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Hosting
{

    /// <summary>
    /// 
    /// </summary>
    public static class IHostBuilderExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseBlazorAuthentication(this IHostBuilder builder)
        {
            return builder;
        }

    }
}
