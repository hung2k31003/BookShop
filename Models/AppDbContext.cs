using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BookShopV2.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName!.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            ConfigureCart(builder);
            ConfigureCategory(builder);
            ConfigureOrder(builder);
            ConfigureOrderItem(builder);
            ConfigurePayment(builder);
            ConfigureShipment(builder);
            ConfigureProduct(builder);
            ConfigureWishlist(builder);
        }

        private void ConfigureCart(ModelBuilder builder)
        {
            builder.Entity<Cart>().ToTable("Cart").HasKey(c => new {c.ProductId,c.CustomerId});
            builder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Cart>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Carts)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureCategory(ModelBuilder builder)
        {
            builder.Entity<Category>().ToTable("Category").HasKey(c => c.CategoryId);
            builder.Entity<Category>()
            .Property(c => c.CategoryId)
            .ValueGeneratedOnAdd();
            builder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
            builder.Entity<Category>()
                .HasIndex(c => c.Slug).IsUnique();
        }

        private void ConfigureOrder(ModelBuilder builder)
        {
            builder.Entity<Order>().ToTable("Order").HasKey(o => o.OrderId);
            builder.Entity<Order>()
            .Property(o => o.OrderId)
            .ValueGeneratedOnAdd();
            builder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);
            builder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PaymentId);
            builder.Entity<Order>()
                .HasOne(o => o.Shipment)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ShipmentId);
            builder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);
        }

        private void ConfigureOrderItem(ModelBuilder builder)
        {
            builder.Entity<OrderItem>().ToTable("OrderItem").HasKey(oi => new { oi.OrderId, oi.ProductId });
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);
        }

        private void ConfigurePayment(ModelBuilder builder)
        {
            builder.Entity<Payment>().ToTable("Payment").HasKey(p => p.PaymentId);
            builder.Entity<Payment>()
            .Property(p=>p.PaymentId)
            .ValueGeneratedOnAdd();
            builder.Entity<Payment>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CustomerId);
            builder.Entity<Payment>()
                .HasMany(p => p.Orders)
                .WithOne(o => o.Payment)
                .HasForeignKey(o => o.PaymentId);
        }

        private void ConfigureShipment(ModelBuilder builder)
        {
            builder.Entity<Shipment>().ToTable("Shipment").HasKey(s => s.ShipmentId);
            builder.Entity<Shipment>()
            .Property(s=>s.ShipmentId)
            .ValueGeneratedOnAdd();
            builder.Entity<Shipment>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Shipments)
                .HasForeignKey(s => s.CustomerId);
            builder.Entity<Shipment>()
                .HasMany(s => s.Orders)
                .WithOne(o => o.Shipment)
                .HasForeignKey(o => o.ShipmentId);
        }

        private void ConfigureProduct(ModelBuilder builder)
        {
            builder.Entity<Product>().ToTable("Product").HasKey(p => p.ProductId);
            builder.Entity<Product>()
            .Property(p=>p.ProductId)
            .ValueGeneratedOnAdd();
            builder.Entity<Product>().HasIndex(p => p.Slug).IsUnique();
            builder.Entity<Product>()
                .HasMany(p => p.Carts)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.ProductId);
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            builder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId);
            builder.Entity<Product>()
                .HasMany(p => p.Wishlists)
                .WithOne(w => w.Product)
                .HasForeignKey(w => w.ProductId);
        }

        private void ConfigureWishlist(ModelBuilder builder)
        {
            builder.Entity<Wishlist>().ToTable("Wishlist").HasKey(w => new {w.ProductId,w.CustomerId});
            builder.Entity<Wishlist>()
                .HasOne(w => w.Product)
                .WithMany(p => p.Wishlists)
                .HasForeignKey(w => w.ProductId);
            builder.Entity<Wishlist>()
                .HasOne(w => w.Customer)
                .WithMany(c => c.Wishlists)
                .HasForeignKey(w => w.CustomerId);
        }

        // DbSet cho các thực thể
        public virtual DbSet<AppUser> Users { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }
    }
}

