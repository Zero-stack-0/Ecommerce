using Service.Dto;
using Service.Dto.Request.Product;
using Service.Dto.Response;
using Service.Helper;

namespace Service.Interface
{
    public interface IProductService
    {
        Task<ApiResponse> Add(AddRequest dto);
        Task<ICollection<ProductResponse>> GetOpenList(string searchTerm, int categoryId);
        Task<ApiResponse> GetListByCreatedById(string searchTerm, int categoryId, UserResponse requestor);
        Task<ApiResponse> GetById(long id);
    }
}