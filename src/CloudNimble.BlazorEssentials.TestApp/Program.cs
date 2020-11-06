using CloudNimble.BlazorEssentials.Authentication;
using CloudNimble.BlazorEssentials.TestApp.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
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
            builder.RootComponents.Add<App>("app");

            var config = builder.Configuration.GetSection("TestApp").Get<ConfigurationBase>();

            builder.Services.AddScoped<ApiAuthorizationMessageHandler>();

            builder.Services.AddHttpClient("BurnRateApi", client => client.BaseAddress = new Uri(config.ApiRoot))
                .AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

            builder.Services.AddSingleton(c => config);

            builder.Services.AddSingleton<IndexViewModel>();
            builder.Services.AddSingleton<SecuredPageViewModel>();
            builder.Services.AddSingleton<AdminPageViewModel>();
            builder.Services.AddSingleton(ConfigurationHelper<ConfigurationBase>.GetConfigurationFromJson());
            builder.Services.AddSingleton<AppStateBase>();

            await builder.Build().RunAsync().ConfigureAwait(false);
        }

    }

}
