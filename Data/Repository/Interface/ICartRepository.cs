using Entities.Models;

namespace Data.Repository.Interface
{
    public interface ICartRepository : IBaseRepository
    {
        Task<(ICollection<Cart>, int totalCount)> GetByAddedById(long addedById, string searchTerm, int pageNo, int pageSize);
        Task<Cart?> GetByProdcutIdAndAddedById(long productId, long addedById);
    }
}