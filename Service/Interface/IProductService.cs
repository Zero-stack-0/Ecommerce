using Service.Dto.Request.Product;
using Service.Helper;

namespace Service.Interface
{
    public interface IProductService
    {
        Task<ApiResponse> Add(AddRequest dto);
    }
}