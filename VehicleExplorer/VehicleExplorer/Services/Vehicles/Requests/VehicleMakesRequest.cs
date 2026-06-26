namespace VehicleExplorer.Web.Services.Vehicles.Requests
{
    public record VehicleMakesRequest(int Page = 1,int PageSize = 10,string? Search = null);
}
