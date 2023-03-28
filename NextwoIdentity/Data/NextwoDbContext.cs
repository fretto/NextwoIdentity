using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextwoIdentity.Models;

namespace NextwoIdentity.Data
{
    public class NextwoDbContext:IdentityDbContext//inheritance
    {
        public NextwoDbContext(DbContextOptions<NextwoDbContext> options) : base(options) {//constructor
        
        
        
        }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }

    }
}
