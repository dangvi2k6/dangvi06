using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("motorcycle")]
public class Motorcycle
{
    [Key]
    [Column("motorcycle_id")]
    public int MotorcycleId { get; set; }

    [Required(ErrorMessage = "Tên Model không được để trống")]
    [Column("model_name")]
    [StringLength(100)]
    public string ModelName { get; set; } = string.Empty;

    [Column("brand_id")]
    public int BrandId { get; set; }

    [Column("type_id")]
    public int? TypeId { get; set; }

    [Column("engine_capacity")]
    public int? EngineCapacity { get; set; }

    [Column("year_from")]
    public int? YearFrom { get; set; }

    [Column("year_to")]
    public int? YearTo { get; set; }

    [Required]
    [Column("price", TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column("color")]
    [StringLength(50)]
    public string? Color { get; set; }

    [Column("warranty_policy")]
    [StringLength(255)]
    public string? WarrantyPolicy { get; set; }

    [Column("image_path")]
    [StringLength(255)]
    public string? ImagePath { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("stock_qty")]
    public int StockQty { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("BrandId")]
    public virtual Brand? Brand { get; set; }

    [ForeignKey("TypeId")]
    public virtual VehicleType? Type { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
}