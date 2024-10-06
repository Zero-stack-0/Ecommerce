using Microsoft.AspNetCore.Http;
using static Entities.Constants;

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

    public static class ResponseMessage
    {
        public static ApiResponse RequestorDoesNotExists()
        {
            return new ApiResponse(null, StatusCodes.Status200OK, Keys.REQUESTOR_DOES_NOT_EXISTS, null);
        }
        public static ApiResponse Sucess(object? data, string message, PagedData? pagedData)
        {
            return new ApiResponse(data, StatusCodes.Status200OK, message, pagedData);
        }
        public static ApiResponse BadRequest(string message)
        {
            return new ApiResponse(null, StatusCodes.Status400BadRequest, message, null);
        }
    }
}
