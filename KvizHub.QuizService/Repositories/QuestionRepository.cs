using Microsoft.EntityFrameworkCore;
using KvizHub.QuizService.Data;
using KvizHub.QuizService.Models.Entities;

namespace KvizHub.QuizService.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuizDbContext _context;

        public QuestionRepository(QuizDbContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetByQuizIdAsync(int quizId)
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .Where(q => q.QuizId == quizId)
                .OrderBy(q => q.Order)
                .ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Question> CreateAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

                        public async Task ReplaceAsync(Question question)
                        {
                            // Load old answers that are tracked
                            var oldAnswers = await _context.Answers
                                .Where(a => a.QuestionId == question.Id)
                                .ToListAsync();

                            // Remove old answers (from change tracker)
                            _context.Answers.RemoveRange(oldAnswers);

                            // Update question scalar properties
                            var existing = await _context.Questions.FindAsync(question.Id);
                            if (existing != null)
                            {
                                existing.Text = question.Text;
                                existing.QuestionType = question.QuestionType;
                                existing.DifficultyLevel = question.DifficultyLevel;
                                existing.Order = question.Order;
                            }

                            // Add new answers
                            foreach (var answer in question.Answers)
                            {
                                _context.Answers.Add(new Answer
                                {
                                    QuestionId = question.Id,
                                    Text = answer.Text,
                                    IsCorrect = answer.IsCorrect,
                                    Order = answer.Order
                                });
                            }

                            await _context.SaveChangesAsync();
                        }

        public async Task DeleteAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }
    }
}
