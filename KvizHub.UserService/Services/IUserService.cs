using KvizHub.UserService.Models.DTOs;

namespace KvizHub.UserService.Services
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetByIdAsync(int id);
        Task<List<UserProfileDto>> GetAllAsync();
        Task<UserProfileDto?> UpdateProfileAsync(int id, UpdateProfileDto dto);
    }
}
