using Microsoft.EntityFrameworkCore;
using WEB_API.Models;
using WMS_API.Models;

namespace WMS_API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ) : base(options)
        {
            
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Accounts> Accounts {  get; set; }
        public DbSet<ResetPasswordLink> ResetPasswordLinks { get; set; }


        
    }
}
