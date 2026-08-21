using KvizHub.UserService.Models.Entities;

namespace KvizHub.UserService.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task CreateAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
        Task RevokeAllForUserAsync(int userId);
        Task DeleteExpiredAsync();
    }
}
