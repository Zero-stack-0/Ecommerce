using Entities.Models;

namespace Data.Repository.Interface
{
    public interface ICategoryRepository : IBaseRepository
    {
        Task<Category?> GetByName(string name);
        Task<(ICollection<Category>, int totalCount)> GetList(int pageNo, int pageSize, string searchTerm);
        Task<Category?> GetById(long id);
    }
}