using Entities.Models;

namespace Data.Repository.Interface
{
    public interface ISellerRequestRepository : IBaseRepository
    {
        Task<SellerRequest?> GetByUserId(long userId);
    }
}