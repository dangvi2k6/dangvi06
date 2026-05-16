using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("full_name")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("password_hash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("phone_number")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Required]
    [Column("role")]
    [StringLength(20)]
    public string Role { get; set; } = "customer";

    // =========================
    // ADMIN USER MANAGEMENT
    // =========================

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [Column("gender")]
    [StringLength(10)]
    public string? Gender { get; set; }

    [Column("birth_day")]
    public int? BirthDay { get; set; }

    [Column("birth_month")]
    public int? BirthMonth { get; set; }

    [Column("birth_year")]
    public int? BirthYear { get; set; }

    [Column("avatar_path")]
    [StringLength(255)]
    public string? AvatarPath { get; set; }


    // =========================
    // NAVIGATION
    // =========================

    public virtual Cart? Cart { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; }
        = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; }
        = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; }
        = new List<Review>();

    public virtual ICollection<Wishlist> Wishlists { get; set; }
        = new List<Wishlist>();
}
