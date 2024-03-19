using Chatpoc.Models;
using Microsoft.EntityFrameworkCore;

namespace Chatpoc.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<HelloWorld> HelloWorld { get; set; }
        public DbSet<User> User { get; set; }



    }
}
