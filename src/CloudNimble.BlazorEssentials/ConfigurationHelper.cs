using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigurationHelper<T> where T : ConfigurationBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T GetConfigurationFromJson(string fileName = "appSettings.json")
        {
            try
            {

                // Get the configuration from embedded dll.
                using (var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(fileName))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return JsonSerializer.Parse<T>(reader.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

    }

}