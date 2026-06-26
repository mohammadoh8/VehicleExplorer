namespace VehicleExplorer.Web.Infrastructure
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseApplicationMiddleware(
            this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Vehicles}/{action=Index}")
                .WithStaticAssets();
            return app;
        }
    }
}
