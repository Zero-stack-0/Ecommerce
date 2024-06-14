namespace Service.Helper
{
    public class ApiResponse
    {
        public ApiResponse(object? result, int statusCodes, string message)
        {
            Result = result;
            StatusCodes = statusCodes;
            Message = message;
        }
        public Object? Result { get; set; }
        public int StatusCodes { get; set; }
        public string Message { get; set; }
    }
}