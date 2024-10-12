using Microsoft.EntityFrameworkCore;
using Entities.Models;
namespace Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext()
        { }

        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        { }

        //define tables

        public DbSet<Users> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<ApiLogs> ApiLogs { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<UserPasswordResetRequest> UserPasswordResetRequest { get; set; }
        public DbSet<SellerRequest> SellerRequest { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<SellerStoreInfo> SellerStoreInfo { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Order { get; set; }
    }
}