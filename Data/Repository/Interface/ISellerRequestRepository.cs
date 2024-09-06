using Entities.Models;

namespace Data.Repository.Interface
{
    public interface ISellerRequestRepository : IBaseRepository
    {
        Task<SellerRequest?> GetById(long id);
        Task<SellerRequest?> GetByUserId(long userId);
        Task<(ICollection<SellerRequest>, int totalCount)> GetRequests(int pageNo, int pageSize, string searchTerm, SellerReqeustStatus? status);
    }
}