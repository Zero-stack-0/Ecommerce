using Service.Dto;
using Service.Dto.Request;
using Service.Helper;

namespace Service.Interface
{
    public interface ISellerService
    {
        Task<ApiResponse> Request(SellerRequestDto dto);
        Task<ApiResponse> Get(UserResponse? requestor);
    }
}