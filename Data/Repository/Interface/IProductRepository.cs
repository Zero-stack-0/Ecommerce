using Entities.Models;

namespace Data.Repository.Interface
{
    public interface IProductRepository : IBaseRepository
    {
        Task<Product?> GetByUserIdAndSku(long userId, string sku);
    }
}