using KvizHub.UserService.Models.DTOs;
using KvizHub.UserService.Repositories;

namespace KvizHub.UserService.Services
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserServiceImpl(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<List<UserProfileDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserProfileDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                ProfileImageUrl = u.ProfileImageUrl,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            }).ToList();
        }

        public async Task<UserProfileDto?> UpdateProfileAsync(int id, UpdateProfileDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            if (!string.IsNullOrEmpty(dto.Username) && dto.Username != user.Username)
            {
                if (await _userRepository.UsernameExistsAsync(dto.Username))
                    throw new InvalidOperationException("Korisničko ime je zauzeto.");
                user.Username = dto.Username;
            }

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                if (await _userRepository.EmailExistsAsync(dto.Email))
                    throw new InvalidOperationException("Email adresa je već registrovana.");
                user.Email = dto.Email;
            }

            if (dto.ProfileImageUrl != null)
                user.ProfileImageUrl = dto.ProfileImageUrl;

            await _userRepository.UpdateAsync(user);

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
