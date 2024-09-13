using Entities.Models;

namespace Data.Repository.Interface
{
    public interface IProductRepository : IBaseRepository
    {
        Task<Product?> GetByUserIdAndSku(long userId, string sku);
        Task<ICollection<Product>> GetList(string searchTerm, int categoryId);
        Task<ICollection<Product>> GetListByCreatedById(string searchTerm, int categoryId, long requestorId);
    }
}