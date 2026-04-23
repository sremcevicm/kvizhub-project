using Microsoft.EntityFrameworkCore;
using KvizHub.UserService.Data;
using KvizHub.UserService.Models.Entities;

namespace KvizHub.UserService.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly UserDbContext _context;

        public RefreshTokenRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);
        }

        public async Task CreateAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAllForUserAsync(int userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpiredAsync()
        {
            var expired = await _context.RefreshTokens
                .Where(rt => rt.ExpiryDate < DateTime.UtcNow || rt.IsRevoked)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(expired);
            await _context.SaveChangesAsync();
        }
    }
}
