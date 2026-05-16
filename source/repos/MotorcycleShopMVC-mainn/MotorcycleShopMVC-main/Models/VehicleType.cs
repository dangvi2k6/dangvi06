using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("vehicle_type")]
public class VehicleType
{
    [Key]
    [Column("type_id")]
    public int TypeId { get; set; }

    [Required]
    [Column("type_name")]
    [StringLength(50)]
    public string TypeName { get; set; } = string.Empty;

    public virtual ICollection<Motorcycle> Motorcycles { get; set; } = new List<Motorcycle>();
}