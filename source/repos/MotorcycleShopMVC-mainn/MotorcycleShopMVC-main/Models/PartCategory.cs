using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("part_category")]
public class PartCategory
{
    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Tên danh mục phụ tùng không được để trống")]
    [Column("category_name")]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
}