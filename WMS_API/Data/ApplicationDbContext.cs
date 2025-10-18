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

        // Foods Table 
        public DbSet<Foods> Foods { get; set; }
        public DbSet<FoodCategories> FoodCategories { get; set; }
        public DbSet<FoodPackage> FoodPackages { get; set; }
        public DbSet<FoodMenu> FoodMenus { get; set; }
        public DbSet<Orders> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional if you use Fluent API to define keys
            modelBuilder.Entity<Accounts>().HasKey(a => a.UsersId);
            modelBuilder.Entity<Users>().HasKey(u => u.Id);
        }

    }
}
