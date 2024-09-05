using Service.Dto;
using Service.Dto.Request;
using Service.Dto.Request.Admin;
using Service.Helper;

namespace Service.Interface
{
    public interface ISellerService
    {
        Task<ApiResponse> Request(SellerRequestDto dto);
        Task<ApiResponse> Get(UserResponse? requestor);
        Task<ApiResponse> UpdateRequestStatus(UpdateSellerRequestStatus dto);
        Task<ApiResponse> GetRequestList(GetSellerRequestList dto);
    }
}