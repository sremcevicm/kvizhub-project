using KvizHub.QuizService.Models.DTOs;
using KvizHub.QuizService.Models.Entities;
using KvizHub.QuizService.Repositories;

namespace KvizHub.QuizService.Services
{
    public interface IQuestionService
    {
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
            var question = await _questionRepository.GetByIdAsync(id);
            if (question == null) return null;

            if (dto.Text != null) question.Text = dto.Text;
            if (dto.QuestionType != null) question.QuestionType = dto.QuestionType;
            if (dto.DifficultyLevel != null) question.DifficultyLevel = dto.DifficultyLevel;
            if (dto.Order.HasValue) question.Order = dto.Order.Value;

            if (dto.Answers != null)
            {
                question.Answers = dto.Answers.Select(a => new Answer
                {
                    QuestionId = question.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect,
                    Order = a.Order
                }).ToList();
            }

            await _questionRepository.UpdateAsync(question);
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
