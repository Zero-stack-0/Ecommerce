using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly EcommerceDbContext context;
        public ProductRepository(EcommerceDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Product?> GetByUserIdAndSku(long userId, string sku)
        {
            return await context.Product.FirstOrDefaultAsync(it => it.SKU == sku && it.CreatedById == userId && !it.IsDeleted);
        }
    }
}