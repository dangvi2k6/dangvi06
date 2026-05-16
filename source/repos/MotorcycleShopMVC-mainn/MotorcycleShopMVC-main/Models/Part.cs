using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("part")]
public class Part
{
    [Key]
    [Column("part_id")]
    public int PartId { get; set; }

    [Required(ErrorMessage = "Tên phụ tùng không được để trống")]
    [Column("part_name")]
    [StringLength(150)]
    public string PartName { get; set; } = string.Empty;

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("brand_id")]
    public int? BrandId { get; set; }

    [Required]
    [Column("price", TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column("stock_quantity")]
    public int StockQuantity { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("image_path")]
    [StringLength(255)]
    public string? ImagePath { get; set; }

    [Column("warranty_months")]
    public int? WarrantyMonths { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("BrandId")]
    public virtual Brand? Brand { get; set; }

    [ForeignKey("CategoryId")]
    public virtual PartCategory? Category { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    public virtual ICollection<Motorcycle> Motorcycles { get; set; } = new List<Motorcycle>();
}