using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly EcommerceDbContext context;
        public UserRepository(EcommerceDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Users?> GetByEmailId(string emailId)
        {
            return await context.Users
            .Include(it => it.Role)
            .Include(it => it.Country)
            .Include(it => it.State)
            .Include(it => it.City)
            .Include(it => it.SellerRequest)
            .FirstOrDefaultAsync(it => it.EmailId == emailId);
        }

        public async Task<Users?> GetByUserName(string userName)
        {
            return await context.Users.FirstOrDefaultAsync(it => it.Username == userName);
        }

        public async Task<Users?> GetByUserNameOrEmailId(string emailOrUserName)
        {
            return await context.Users
            .Include(it => it.Role)
            .Include(it => it.SellerRequest)
            .FirstOrDefaultAsync(it => (it.EmailId == emailOrUserName || it.Username == emailOrUserName) && it.IsActive && !it.IsDeleted);
        }

        public async Task<Country?> GetCountry(long id)
        {
            return await context.Country
            .Include(s => s.State)
                .ThenInclude(c => c.City)
            .FirstOrDefaultAsync(it => it.Id == id);
        }

        public async Task<(ICollection<Users>, int totalCount)> UsersList(int pageNo, int pageSize, string searchTerm, long requestorId)
        {
            IQueryable<Users> query = context.Users
                .Include(it => it.Country)
                .Include(it => it.State)
                .Include(it => it.City)
                .Where(it => it.Id != requestorId)
                .OrderByDescending(it => it.Id);


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(it => (it.FirstName + " " + it.LastName).Contains(searchTerm) || it.Username.Contains(searchTerm) || it.EmailId.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var users = await query.Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return (users, totalCount);
        }

        public async Task<Users?> GetUserByVerificationCode(string verificationCode)
        {
            return await context.Users
            .FirstOrDefaultAsync(it => it.AccountVerificationCode == verificationCode);
        }

        public async Task<Users?> GetById(long id)
        {
            return await context.Users
            .FirstOrDefaultAsync(it => it.Id == id);
        }
    }
}