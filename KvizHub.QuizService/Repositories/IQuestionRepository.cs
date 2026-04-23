using KvizHub.QuizService.Models.Entities;

namespace KvizHub.QuizService.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetByQuizIdAsync(int quizId);
        Task<Question?> GetByIdAsync(int id);
        Task<Question> CreateAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteAsync(int id);
    }
}
