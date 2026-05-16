using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("review")]
public class Review
{
    [Key]
    [Column("review_id")]
    public int ReviewId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("motorcycle_id")]
    public int? MotorcycleId { get; set; }

    [Column("part_id")]
    public int? PartId { get; set; }

    [Required]
    [Column("rating")]
    public int Rating { get; set; }

    [Column("comment")]
    public string? Comment { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("MotorcycleId")]
    public virtual Motorcycle? Motorcycle { get; set; }

    [ForeignKey("PartId")]
    public virtual Part? Part { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    public virtual ICollection<ReviewImage> ReviewImages { get; set; } = new List<ReviewImage>();
}