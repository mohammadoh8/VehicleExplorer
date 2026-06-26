using VehicleExplorer.Web.Integrations.ExternalVehicleProvider;
using VehicleExplorer.Web.Integrations.IntegrationClient;
using VehicleExplorer.Web.Integrations.Nhtsa;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddScoped<INhtsaVehicleClient, NhtsaVehicleClient>();
builder.Services.Configure<NhtsaOptions>(
    builder.Configuration.GetSection("IntegrationUrls:Nhtsa"));


builder.Services.AddHttpClient<IApiHttpClient, ApiHttpClient>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
