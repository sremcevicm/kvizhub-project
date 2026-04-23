using Microsoft.EntityFrameworkCore;
using KvizHub.QuizService.Data;
using KvizHub.QuizService.Models.Entities;

namespace KvizHub.QuizService.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizDbContext _context;

        public QuizRepository(QuizDbContext context)
        {
            _context = context;
        }

        public async Task<List<Quiz>> GetAllAsync()
        {
            return await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                .ToListAsync();
        }

        public async Task<List<Quiz>> GetFilteredAsync(int? categoryId, string? difficulty, string? search)
        {
            var query = _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                .Where(q => q.IsActive);

            if (categoryId.HasValue)
                query = query.Where(q => q.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(difficulty))
                query = query.Where(q => q.Difficulty == difficulty);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(q =>
                    q.Title.Contains(search) ||
                    (q.Description != null && q.Description.Contains(search)));

            return await query.OrderByDescending(q => q.CreatedAt).ToListAsync();
        }

        public async Task<Quiz?> GetByIdAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Quiz?> GetByIdWithQuestionsAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Category)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Quiz> CreateAsync(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task UpdateAsync(Quiz quiz)
        {
            _context.Quizzes.Update(quiz);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz != null)
            {
                _context.Quizzes.Remove(quiz);
                await _context.SaveChangesAsync();
            }
        }
    }
}
