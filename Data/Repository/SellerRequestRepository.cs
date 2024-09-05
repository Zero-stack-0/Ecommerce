using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class SellerRequestRepository : BaseRepository, ISellerRequestRepository
    {
        private readonly EcommerceDbContext context;
        public SellerRequestRepository(EcommerceDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<SellerRequest?> GetByUserId(long userId)
        {
            return await context.SellerRequest
            .Include(it => it.User)
            .FirstOrDefaultAsync(it => it.UserId == userId);
        }

        public async Task<(ICollection<SellerRequest>, int totalCount)> GetRequests(int pageNo, int pageSize, string searchTerm, SellerReqeustStatus? status)
        {
            IQueryable<SellerRequest> query = context.SellerRequest
            .Include(it => it.User)
            .OrderByDescending(it => it.Id);


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(it => (it.User.FirstName + " " + it.User.LastName).Contains(searchTerm));
            }

            if (status is not null)
            {
                query = query.Where(it => it.Status == status);
            }

            var totalCount = await query.CountAsync();

            var requests = await query.Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (requests, totalCount);
        }
    }
}