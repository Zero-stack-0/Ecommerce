using Entities.Models;
using Service.Dto;
using Service.Helper;

namespace Service.Interface
{
    public interface IAccountService
    {
        Task<ApiResponse> Create(SignUpRequest dto);
        Task<ApiResponse> GetUserByLoginCredential(LoginRequest dto);
        Task<UserResponse?> GetUserProfile(string emailId);
        Task<Country?> GetCountry(long id);
    }
}