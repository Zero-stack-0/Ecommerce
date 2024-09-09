using Entities.Models;

namespace Data.Repository.Interface
{
    public interface ISellerStoreInfoRepository : IBaseRepository
    {
        Task<SellerStoreInfo?> GetByUserId(long userId);
    }
}