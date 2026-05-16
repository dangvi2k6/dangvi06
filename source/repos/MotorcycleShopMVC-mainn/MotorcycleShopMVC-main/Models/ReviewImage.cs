using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("review_image")]
public class ReviewImage
{
    [Key]
    [Column("image_id")]
    public int ImageId { get; set; }

    [Column("review_id")]
    public int ReviewId { get; set; }

    [Required]
    [Column("image_path")]
    [StringLength(255)]
    public string ImagePath { get; set; } = string.Empty;

    [ForeignKey("ReviewId")]
    public virtual Review? Review { get; set; }
}