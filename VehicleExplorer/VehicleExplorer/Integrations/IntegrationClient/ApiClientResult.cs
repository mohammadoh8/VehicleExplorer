namespace VehicleExplorer.Web.Integrations.IntegrationClient
{
    using System.Net;

    public sealed record ApiClientResult<T>
    {
        public bool IsSuccess { get; init; }

        public T? Data { get; init; }

        public string? ErrorMessage { get; init; }

        public HttpStatusCode? StatusCode { get; init; }

        public static ApiClientResult<T> Success(T data, HttpStatusCode statusCode)
        {
            return new ApiClientResult<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static ApiClientResult<T> Failure(
            string errorMessage,
            HttpStatusCode? statusCode = null)
        {
            return new ApiClientResult<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }
    }
}
