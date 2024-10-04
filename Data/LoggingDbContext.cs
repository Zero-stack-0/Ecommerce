using Microsoft.EntityFrameworkCore;
using Entities.Models;
namespace Data
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext()
        { }

        public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options)
        { }

        //define tables
        public DbSet<LoggingException> LoggingException { get; set; }
    }
}