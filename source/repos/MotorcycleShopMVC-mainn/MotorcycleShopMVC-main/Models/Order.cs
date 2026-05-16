using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("orders")]
public class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("order_date", TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [Required]
    [Column("total_amount", TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = "pending";

    [Column("shipping_address")]
    [StringLength(255)]
    public string? ShippingAddress { get; set; }

    [Column("payment_status")]
    [StringLength(20)]
    public string? PaymentStatus { get; set; }

    [Column("payment_method")]
    [StringLength(50)]
    public string? PaymentMethod { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}