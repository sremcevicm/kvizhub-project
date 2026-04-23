using KvizHub.QuizService.Models.Entities;

namespace KvizHub.QuizService.Repositories
{
    public interface IQuizRepository
    {
        Task<List<Quiz>> GetAllAsync();
        Task<List<Quiz>> GetFilteredAsync(int? categoryId, string? difficulty, string? search);
        Task<Quiz?> GetByIdAsync(int id);
        Task<Quiz?> GetByIdWithQuestionsAsync(int id);
        Task<Quiz> CreateAsync(Quiz quiz);
        Task UpdateAsync(Quiz quiz);
        Task DeleteAsync(int id);
    }
}
