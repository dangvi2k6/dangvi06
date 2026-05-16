using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("cart_item")]
public class CartItem
{
    [Key]
    [Column("cart_item_id")]
    public int CartItemId { get; set; }

    [Column("cart_id")]
    public int CartId { get; set; }

    [Column("motorcycle_id")]
    public int? MotorcycleId { get; set; }

    [Column("part_id")]
    public int? PartId { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [ForeignKey("CartId")]
    public virtual Cart? Cart { get; set; }

    [ForeignKey("MotorcycleId")]
    public virtual Motorcycle? Motorcycle { get; set; }

    [ForeignKey("PartId")]
    public virtual Part? Part { get; set; }
}