using System.ComponentModel.DataAnnotations;

namespace KvizHub.UserService.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }

        [Required, MaxLength(20)]
        public string Role { get; set; } = "User"; // "User" or "Admin"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
