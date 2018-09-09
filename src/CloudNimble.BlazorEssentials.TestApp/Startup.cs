using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CloudNimble.BlazorEssentials.TestApp
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new ConfigurationBase { ApiRoot = "https://catalog.data.gov/api/3/" });
            services.AddSingleton<AppStateBase>();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }

    }

}