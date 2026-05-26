using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KvizHub.UserService.Models.DTOs
{
    public class RegisterRequestDto
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }
    }

    public class LoginRequestDto
    {
        // Accepts "usernameOrEmail" from JSON
        public string? UsernameOrEmail { get; set; }

        // Also accepts "username" from JSON (fallback)
        public string? Username { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        // Resolved value: prefer UsernameOrEmail, fall back to Username
        [JsonIgnore]
        public string ResolvedLogin => !string.IsNullOrEmpty(UsernameOrEmail) 
            ? UsernameOrEmail 
            : Username ?? string.Empty;
    }

    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateProfileDto
    {
        [MaxLength(50)]
        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? ProfileImageUrl { get; set; }
    }
}
