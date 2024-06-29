using Entities.Models;

namespace Data.Repository.Interface
{
    public interface IUserPasswordResetRequestRepository : IBaseRepository
    {
        Task<(ICollection<UserPasswordResetRequest> requests, int totalCountOfRequestInDay)> GetByUserId(long userId);
        Task<UserPasswordResetRequest?> GetByToken(string token);
    }
}