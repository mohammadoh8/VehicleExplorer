namespace VehicleExplorer.Web.Shared
{
    public record OperationResult<T>
    {
        public bool IsSuccess { get; init; }

        public T? Data { get; init; }

        public string? ErrorMessage { get; init; }

        public static OperationResult<T> Success(T data)
        {
            return new OperationResult<T>
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static OperationResult<T> Failure(
            string errorMessage)
        {
            return new OperationResult<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }
}
