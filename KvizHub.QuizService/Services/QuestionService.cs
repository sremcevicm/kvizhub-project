using KvizHub.QuizService.Models.DTOs;
using KvizHub.QuizService.Models.Entities;
using KvizHub.QuizService.Repositories;

namespace KvizHub.QuizService.Services
{
        public interface IQuestionService
    {
        Task<QuestionDto?> GetByIdAsync(int id);
        Task<List<QuestionDto>> GetByQuizIdAsync(int quizId);
        Task<QuestionDto> CreateAsync(int quizId, CreateQuestionDto dto);
        Task<QuestionDto?> UpdateAsync(int id, UpdateQuestionDto dto);
        Task DeleteAsync(int id);
    }

        public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<QuestionDto?> GetByIdAsync(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            return question == null ? null : MapToDto(question);
        }

        public async Task<List<QuestionDto>> GetByQuizIdAsync(int quizId)
        {
            var questions = await _questionRepository.GetByQuizIdAsync(quizId);
            return questions.Select(MapToDto).ToList();
        }

        public async Task<QuestionDto> CreateAsync(int quizId, CreateQuestionDto dto)
        {
            var question = new Question
            {
                QuizId = quizId,
                Text = dto.Text,
                QuestionType = dto.QuestionType,
                DifficultyLevel = dto.DifficultyLevel,
                Order = dto.Order,
                Answers = dto.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect,
                    Order = a.Order
                }).ToList()
            };

            await _questionRepository.CreateAsync(question);
            var created = await _questionRepository.GetByIdAsync(question.Id);
            return MapToDto(created!);
        }

                public async Task<QuestionDto?> UpdateAsync(int id, UpdateQuestionDto dto)
        {
            // Load existing to verify it exists
            var existing = await _questionRepository.GetByIdAsync(id);
            if (existing == null) return null;

            // Build a replacement question with updated values
            var replacement = new Question
            {
                Id = id,
                Text = dto.Text ?? existing.Text,
                QuestionType = dto.QuestionType ?? existing.QuestionType,
                DifficultyLevel = dto.DifficultyLevel ?? existing.DifficultyLevel,
                Order = dto.Order ?? existing.Order,
                QuizId = existing.QuizId
            };

            if (dto.Answers != null)
            {
                replacement.Answers = dto.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect,
                    Order = a.Order
                }).ToList();
            }
            else
            {
                // Keep existing answers
                replacement.Answers = existing.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect,
                    Order = a.Order
                }).ToList();
            }

            await _questionRepository.ReplaceAsync(replacement);
            var updated = await _questionRepository.GetByIdAsync(id);
            return MapToDto(updated!);
        }

        public async Task DeleteAsync(int id)
        {
            await _questionRepository.DeleteAsync(id);
        }

        private static QuestionDto MapToDto(Question q) => new()
        {
            Id = q.Id,
            QuizId = q.QuizId,
            Text = q.Text,
            QuestionType = q.QuestionType,
            DifficultyLevel = q.DifficultyLevel,
            Order = q.Order,
            Answers = q.Answers.Select(a => new AnswerDto
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect,
                Order = a.Order
            }).OrderBy(a => a.Order).ToList()
        };
    }
}
