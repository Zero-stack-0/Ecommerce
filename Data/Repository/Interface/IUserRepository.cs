using Entities.Models;

namespace Data.Repository.Interface
{
    public interface IUserRepository : IBaseRepository
    {
        Task<Users?> GetByEmailId(string emailId);
        Task<Users?> GetByUserName(string userName);
        Task<Users?> GetByUserNameOrEmailId(string emailOrUserName);
        Task<Country?> GetCountry(long id);
        Task<(ICollection<Users>, int totalCount)> UsersList(int pageNo, int pageSize, string searchTerm, long requestorId);
        Task<Users?> GetUserByVerificationCode(string verificationCode);
    }
}