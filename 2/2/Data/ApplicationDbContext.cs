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
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        // Метод конфігурації контексту бази даних
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Використовуйте цей метод, лише якщо не використовуєте Dependency Injection для рядка підключення
                optionsBuilder.UseSqlServer("Your_Connection_String_Here", b => b.MigrationsAssembly("_2"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування зв’язків, якщо необхідно
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.OrderStatus)
                .WithMany(os => os.Orders)
                .HasForeignKey(o => o.OrderStatusId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeliveryMethod)
                .WithMany(dm => dm.Orders)
                .HasForeignKey(o => o.DeliveryMethodId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
            modelBuilder.Entity<CartItem>().HasNoKey();

            modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus { Id = 1, Name = "Очікується" },
            new OrderStatus { Id = 2, Name = "Відхилено" },
            new OrderStatus { Id = 3, Name = "Підтверджено" },
            new OrderStatus { Id = 4, Name = "Відправлено" }
        );
        }
    }
}











