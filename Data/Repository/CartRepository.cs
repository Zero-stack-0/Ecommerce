using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class CartRepository : BaseRepository, ICartRepository
    {
        private readonly EcommerceDbContext context;
        public CartRepository(EcommerceDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<(ICollection<Cart>, int totalCount)> GetByAddedById(long addedById, string searchTerm, int pageNo, int pageSize)
        {

            IQueryable<Cart> query = context.Cart
                .Include(it => it.Product)
                .Where(it => it.AddedById == addedById && !it.IsDeleted)
                .OrderByDescending(it => it.Id);


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(it => it.Product.Name.Contains(searchTerm) || it.Product.Description.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var users = await query.Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (users, totalCount);
        }

        public async Task<Cart?> GetByProdcutIdAndAddedById(long productId, long addedById)
        {
            return await context.Cart.FirstOrDefaultAsync(it => it.ProductId == productId && it.AddedById == addedById && !it.IsDeleted);
        }
    }
}