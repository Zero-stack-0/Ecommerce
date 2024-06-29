using Azure.Core;
using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class UserPasswordResetRequestRepository : BaseRepository, IUserPasswordResetRequestRepository
    {
        private readonly EcommerceDbContext context;
        public UserPasswordResetRequestRepository(EcommerceDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<(ICollection<UserPasswordResetRequest> requests, int totalCountOfRequestInDay)> GetByUserId(long userId)
        {
            IQueryable<UserPasswordResetRequest> query = context.UserPasswordResetRequest
                .Where(it => it.UserId == userId);

            var requestsOfToday = query.Where(e => e.RequestedAt > DateTime.UtcNow.AddDays(-1));
            int requestOfTodayCount = await requestsOfToday.CountAsync();

            return (await query.ToListAsync(), requestOfTodayCount);
        }

        public async Task<UserPasswordResetRequest?> GetByToken(string token)
        {
            return await context.UserPasswordResetRequest
            .FirstOrDefaultAsync(it => it.ResetToken == token && !it.IsUsed && !it.IsDeleted);
        }
    }
}