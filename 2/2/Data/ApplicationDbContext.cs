using Microsoft.EntityFrameworkCore;
using _2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace _2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<> Categories { get; set; }

        // Метод налаштування контексту бази даних
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "DefaultConnection"; 
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("_2"));
            }
        }
    }
}










