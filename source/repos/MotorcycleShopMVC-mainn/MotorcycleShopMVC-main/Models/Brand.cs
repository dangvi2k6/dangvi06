using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("brand")]
public class Brand
{
    [Key]
    [Column("brand_id")]
    public int BrandId { get; set; }

    [Required(ErrorMessage = "Tên hãng không được để trống")]
    [Column("brand_name")]
    [StringLength(100)]
    public string BrandName { get; set; } = string.Empty;

    [Column("country")]
    [StringLength(100)]
    public string? Country { get; set; }

    public virtual ICollection<Motorcycle> Motorcycles { get; set; } = new List<Motorcycle>();
    public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
}