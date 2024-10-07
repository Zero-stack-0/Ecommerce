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

        public async Task<ICollection<Product>> GetList(string searchTerm, int categoryId, long? requestorId)
        {
            IQueryable<Product> query = context.Product
            .Where(it => !it.IsDeleted && (requestorId == null ? true : it.CreatedById != requestorId))
            .OrderByDescending(it => it.Id);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(it => (it.Name + " " + it.Description).Contains(searchTerm));
            }

            if (categoryId > 0)
            {
                query = query.Where(it => it.CategoryId == categoryId);
            }

            return await query.ToListAsync();
        }

        public async Task<ICollection<Product>> GetListByCreatedById(string searchTerm, int categoryId, long requestorId)
        {
            IQueryable<Product> query = context.Product
            .Where(it => it.CreatedById == requestorId)
            .OrderByDescending(it => it.Id);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(it => (it.Name + " " + it.Description).Contains(searchTerm));
            }

            if (categoryId > 0)
            {
                query = query.Where(it => it.CategoryId == categoryId);
            }

            return await query.ToListAsync();
        }

        public async Task<Product?> GetById(long id)
        {
            return await context.Product
            .Include(it => it.Category)
            .FirstOrDefaultAsync(it => it.Id == id && !it.IsDeleted);
        }
    }
}