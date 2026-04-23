using Microsoft.EntityFrameworkCore;
using KvizHub.ScoreService.Data;
using KvizHub.ScoreService.Models.Entities;

namespace KvizHub.ScoreService.Repositories
{
    public interface IAttemptRepository
    {
        Task<QuizAttempt> CreateAsync(QuizAttempt attempt);
        Task<QuizAttempt?> GetByIdAsync(int id);
        Task<List<QuizAttempt>> GetByUserIdAsync(int userId);
        Task<List<QuizAttempt>> GetByQuizIdAsync(int quizId);
        Task<List<QuizAttempt>> GetAllAsync();
    }

    public class AttemptRepository : IAttemptRepository
    {
        private readonly ScoreDbContext _context;

        public AttemptRepository(ScoreDbContext context)
        {
            _context = context;
        }

        public async Task<QuizAttempt> CreateAsync(QuizAttempt attempt)
        {
            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();
            return attempt;
        }

        public async Task<QuizAttempt?> GetByIdAsync(int id)
        {
            return await _context.QuizAttempts
                .Include(a => a.Answers)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<QuizAttempt>> GetByUserIdAsync(int userId)
        {
            return await _context.QuizAttempts
                .Include(a => a.Answers)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CompletedAt)
                .ToListAsync();
        }

        public async Task<List<QuizAttempt>> GetByQuizIdAsync(int quizId)
        {
            return await _context.QuizAttempts
                .Include(a => a.Answers)
                .Where(a => a.QuizId == quizId)
                .OrderByDescending(a => a.Score)
                .ThenBy(a => a.TimeTakenSeconds)
                .ToListAsync();
        }

        public async Task<List<QuizAttempt>> GetAllAsync()
        {
            return await _context.QuizAttempts
                .Include(a => a.Answers)
                .OrderByDescending(a => a.CompletedAt)
                .ToListAsync();
        }
    }
}
