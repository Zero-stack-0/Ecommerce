using Service.Dto;
using Service.Dto.Request.Order;
using Service.Dto.Response;
using Service.Helper;

namespace Service.Interface
{
    public interface IOrderService
    {
        Task<ApiResponse> Add(AddRequest dto);
        Task<ApiResponse> GetPaymentIntentId(CreatePaymentIntentRequest dto);
    }
}