using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class SellerStoreInfoRepository : BaseRepository, ISellerStoreInfoRepository
    {
        private readonly EcommerceDbContext context;
        public SellerStoreInfoRepository(EcommerceDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<SellerStoreInfo?> GetByUserId(long userId)
        {
            return await context.SellerStoreInfo.FirstOrDefaultAsync(it => it.UserId == userId);
        }
    }
}