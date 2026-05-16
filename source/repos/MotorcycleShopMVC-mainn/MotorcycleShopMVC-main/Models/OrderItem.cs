using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("order_item")]
public class OrderItem
{
    [Key]
    [Column("order_item_id")]
    public int OrderItemId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("motorcycle_id")]
    public int? MotorcycleId { get; set; }

    [Column("part_id")]
    public int? PartId { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("price", TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [ForeignKey("MotorcycleId")]
    public virtual Motorcycle? Motorcycle { get; set; }

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }

    [ForeignKey("PartId")]
    public virtual Part? Part { get; set; }
}