using Service.Dto;
using Service.Dto.Request.Cart;
using Service.Helper;

namespace Service.Interface;

public interface ICartService
{
    Task<ApiResponse> Add(AddRequest dto);
    Task<ApiResponse> GetList(GetListRequest dto);
    Task<ApiResponse> GetDetail(long cartId, UserResponse? requestor);
    Task<ApiResponse> Update(UpdateRequest dto);
}