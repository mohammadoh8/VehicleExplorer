using FluentValidation;
using VehicleExplorer.Web.Integrations.IntegrationClient;
using VehicleExplorer.Web.Integrations.VehicleProviders;
using VehicleExplorer.Web.Integrations.VehicleProviders.Nhtsa;
using VehicleExplorer.Web.Services.Vehicles;
using VehicleExplorer.Web.Services.Vehicles.Requests;
using VehicleExplorer.Web.Services.Vehicles.Validators;
using VehicleExplorer.Web.Shared;

namespace VehicleExplorer.Web.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheService>();
            services.AddScoped<IValidator<VehicleModelsRequest>, VehicleModelsRequestValidator>();
            services.AddScoped<IVehicleProviderClient, NhtsaVehicleClient>();
            services.AddScoped<IVehicleCatalogService, VehicleCatalogService>();
            services.Configure<NhtsaOptions>(configuration.GetSection("IntegrationUrls:Nhtsa"));

            services.AddHttpClient<IApiHttpClient, ApiHttpClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            return services;
        }
    }
}
