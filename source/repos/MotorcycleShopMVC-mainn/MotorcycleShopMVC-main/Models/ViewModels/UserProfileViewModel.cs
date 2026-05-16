using System.ComponentModel.DataAnnotations;

namespace MotorcycleShopMVC.Models.ViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        public string? FullName { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public string? Gender { get; set; }
        public int? BirthDay { get; set; }
        public int? BirthMonth { get; set; }
        public int? BirthYear { get; set; }

        public string? AvatarPath { get; set; }

        public IFormFile? AvatarFile { get; set; }

    }
}
