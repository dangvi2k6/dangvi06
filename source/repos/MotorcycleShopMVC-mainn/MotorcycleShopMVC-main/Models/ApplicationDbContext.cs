using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MotorcycleShopMVC.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<Motorcycle> Motorcycles { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Part> Parts { get; set; }
    public virtual DbSet<PartCategory> PartCategories { get; set; }
    public virtual DbSet<Promotion> Promotions { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<ReviewImage> ReviewImages { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<VehicleType> VehicleTypes { get; set; }
    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Part>()
            .HasMany(p => p.Motorcycles)
            .WithMany(m => m.Parts)
            .UsingEntity<Dictionary<string, object>>(
                "PartMotorcycle",
                j => j.HasOne<Motorcycle>().WithMany().HasForeignKey("MotorcycleId"),
                j => j.HasOne<Part>().WithMany().HasForeignKey("PartId"),
                j =>
                {
                    j.HasKey("PartId", "MotorcycleId");
                    j.ToTable("part_motorcycle");
                });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Motorcycle>(entity =>
        {
            entity.HasKey(e => e.MotorcycleId);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });
    }
}