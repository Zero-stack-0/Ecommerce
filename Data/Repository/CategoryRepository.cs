using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        private readonly EcommerceDbContext context;
        public CategoryRepository(EcommerceDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Category?> GetByName(string name)
        {
            return await context.Category.FirstOrDefaultAsync(it => it.Name == name);
        }

        public async Task<(ICollection<Category>, int totalCount)> GetList(int pageNo, int pageSize, string searchTerm)
        {
            IQueryable<Category> query = context.Category
            .OrderByDescending(it => it.Id);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(it => it.Name.Contains(searchTerm) || it.Description.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var categories = await query.Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (categories, totalCount);
        }

        public async Task<Category?> GetById(long id)
        {
            return await context.Category.FirstOrDefaultAsync(it => it.Id == id);
        }

        public async Task<ICollection<Category>> GetListForUser()
        {
            return await context.Category.Where(it => !it.IsDeleted).OrderByDescending(it => it.Id).ToListAsync();
        }
    }
}