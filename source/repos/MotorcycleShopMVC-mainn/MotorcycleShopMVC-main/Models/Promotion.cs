using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("promotion")]
public class Promotion
{
    [Key]
    [Column("promotion_id")]
    public int PromotionId { get; set; }

    [Required]
    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("discount_percent", TypeName = "decimal(5, 2)")]
    public decimal DiscountPercent { get; set; }

    [Column("start_date", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; } = true;
}