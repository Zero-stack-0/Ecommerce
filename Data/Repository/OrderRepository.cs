using Data.Repository.Interface;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        private readonly EcommerceDbContext context;
        public OrderRepository(EcommerceDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}