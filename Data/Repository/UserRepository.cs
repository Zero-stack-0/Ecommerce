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
            return await context.Users.FirstOrDefaultAsync(it => it.EmailId == emailId);
        }

        public async Task<Users?> GetByUserName(string userName)
        {
            return await context.Users.FirstOrDefaultAsync(it => it.Username == userName);
        }

        public async Task<Users?> GetByUserNameOrEmailId(string emailOrUserName)
        {
            return await context.Users.FirstOrDefaultAsync(it => (it.EmailId == emailOrUserName || it.Username == emailOrUserName) && it.IsActive && !it.IsDeleted);
        }
    }
}