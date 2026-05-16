using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorcycleShopMVC.Models;

[Table("notification")]
public class Notification
{
    [Key]
    [Column("notification_id")]
    public int NotificationId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("message")]
    public string Message { get; set; } = string.Empty;

    [Column("is_read")]
    public bool? IsRead { get; set; } = false;

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}