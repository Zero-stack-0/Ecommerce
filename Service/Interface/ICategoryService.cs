using Service.Dto;
using Service.Dto.Request.Admin.Category;
using Service.Helper;

namespace Service.Interface
{
    public interface ICategoryService
    {
        Task<ApiResponse> Add(AddCategoryRequest dto);
        Task<ApiResponse> GetList(GetCategoryListRequest dto);
        Task<ApiResponse> Delete(DeleteRequest dto);
        Task<ApiResponse> Update(UpdateRequest dto);
        Task<ApiResponse> GetById(long id, UserResponse requestor);
        Task<ApiResponse> GetListForOwner();
    }
}