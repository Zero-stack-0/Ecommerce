using Entities.Models;

namespace Data.Repository.Interface
{
    public interface IUserRepository : IBaseRepository
    {
        Task<Users?> GetByEmailId(string emailId);
        Task<Users?> GetByUserName(string userName);
        Task<Users?> GetByUserNameOrEmailId(string emailOrUserName);
    }
}