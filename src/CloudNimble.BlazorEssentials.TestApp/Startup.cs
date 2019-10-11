using CloudNimble.BlazorEssentials.TestApp.ViewModels;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CloudNimble.BlazorEssentials.TestApp
{

    /// <summary>
    /// Configures the startup parameters for your app and adds the required services to the DI container.
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Configures the required services for the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to populate with services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IndexViewModel>();
            services.AddSingleton<SecuredPageViewModel>();
            services.AddSingleton<AdminPageViewModel>();
            services.AddSingleton(ConfigurationHelper<ConfigurationBase>.GetConfigurationFromJson());
            services.AddSingleton<BlazorAuthenticationConfig, TestAppAuthenticationConfig>();
            services.AddSingleton<AppStateBase>();

        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }

    }

}