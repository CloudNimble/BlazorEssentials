using Microsoft.JSInterop;
using System.IO;
using System.Reflection;

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
            // Get the configuration from embedded dll.
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
            using (var reader = new StreamReader(stream))
            {
                return Json.Deserialize<T>(reader.ReadToEnd());
            }
        }

    }

}