using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using KvizHub.UserService.Models.DTOs;
using KvizHub.UserService.Models.Entities;
using KvizHub.UserService.Repositories;

namespace KvizHub.UserService.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (await _userRepository.UsernameExistsAsync(request.Username))
                throw new InvalidOperationException("Korisničko ime je zauzeto.");

            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("Email adresa je već registrovana.");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                ProfileImageUrl = request.ProfileImageUrl,
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            var accessToken = GenerateAccessToken(user);
            var refreshToken = await GenerateAndSaveRefreshToken(user.Id);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByUsernameOrEmailAsync(request.ResolvedLogin);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Pogrešno korisničko ime/email ili lozinka.");

            var accessToken = GenerateAccessToken(user);
            var refreshToken = await GenerateAndSaveRefreshToken(user.Id);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var tokenRecord = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (tokenRecord == null || tokenRecord.ExpiryDate < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Nevažeći refresh token.");

            tokenRecord.IsRevoked = true;
            var user = tokenRecord.User;

            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = await GenerateAndSaveRefreshToken(user.Id);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        private string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Secret"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateAndSaveRefreshToken(int userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.CreateAsync(refreshToken);
            return token;
        }
    }
}
