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
    }
}