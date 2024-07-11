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
            .FirstOrDefaultAsync(it => it.UserId == userId);
        }
    }
}