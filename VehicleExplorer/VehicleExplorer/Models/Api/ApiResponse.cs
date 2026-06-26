namespace VehicleExplorer.Web.Models.Api
{
    public record ApiResponse<T>(bool Success,T? Data,string? Message)
    {
        public static ApiResponse<T> Ok(T data)
        {
            return new ApiResponse<T>(true, data, null);
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>(false, default, message);
        }
    }
}
