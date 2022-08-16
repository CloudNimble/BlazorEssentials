using CloudNimble.BlazorEssentials.TestApp.Models;
using CloudNimble.BlazorEssentials.TestApp.ViewModels;
using CloudNimble.EasyAF.Configuration;
using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.TestApp
{

    /// <summary>
    /// 
    /// </summary>
    public class Program
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.AddBlazorEssentials<ConfigurationBase, AppState>("TestApp");

            builder.Services.AddSingleton<IndexViewModel>();
            builder.Services.AddSingleton<LoadingContainerViewModel>();
            builder.Services.AddSingleton<WizardViewModel>();
            builder.Services.AddSingleton<DebounceViewModel>();

            builder.Services.AddHxServices();

            await builder.Build().RunAsync().ConfigureAwait(false);
        }

    }

}
