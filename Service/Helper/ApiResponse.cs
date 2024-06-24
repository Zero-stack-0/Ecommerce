namespace Service.Helper
{
    public class ApiResponse
    {
        public ApiResponse(object? result, int statusCodes, string message, PagedData? pagedData)
        {
            Result = result;
            StatusCodes = statusCodes;
            Message = message;
            PagedData = pagedData;
        }
        public Object? Result { get; set; }
        public int StatusCodes { get; set; }
        public string Message { get; set; }
        public PagedData? PagedData { get; set; }
    }

    public class PagedData
    {
        public PagedData(int pageNo, int pageSize, int totalCount, int totalPages)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}