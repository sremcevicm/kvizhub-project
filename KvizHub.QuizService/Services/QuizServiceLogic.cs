using KvizHub.QuizService.Models.DTOs;
using KvizHub.QuizService.Models.Entities;
using KvizHub.QuizService.Repositories;

namespace KvizHub.QuizService.Services
{
    public interface IQuizServiceLogic
    {
        Task<List<QuizDto>> GetAllAsync();
        Task<List<QuizDto>> GetFilteredAsync(int? categoryId, string? difficulty, string? search);
        Task<QuizDto?> GetByIdAsync(int id);
        Task<List<QuestionDto>> GetQuestionsWithAnswersAsync(int quizId);
        Task<List<QuestionForPlayerDto>> GetQuestionsForPlayerAsync(int quizId);
        Task<QuizDto> CreateAsync(CreateQuizDto dto, int userId);
        Task<QuizDto?> UpdateAsync(int id, UpdateQuizDto dto);
        Task DeleteAsync(int id);
    }

    public class QuizServiceLogic : IQuizServiceLogic
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;

        public QuizServiceLogic(IQuizRepository quizRepository, IQuestionRepository questionRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
        }

        public async Task<List<QuizDto>> GetAllAsync()
        {
            var quizzes = await _quizRepository.GetAllAsync();
            return quizzes.Select(MapToDto).ToList();
        }

        public async Task<List<QuizDto>> GetFilteredAsync(int? categoryId, string? difficulty, string? search)
        {
            var quizzes = await _quizRepository.GetFilteredAsync(categoryId, difficulty, search);
            return quizzes.Select(MapToDto).ToList();
        }

        public async Task<QuizDto?> GetByIdAsync(int id)
        {
            var quiz = await _quizRepository.GetByIdAsync(id);
            return quiz == null ? null : MapToDto(quiz);
        }

        public async Task<List<QuestionDto>> GetQuestionsWithAnswersAsync(int quizId)
        {
            var questions = await _questionRepository.GetByQuizIdAsync(quizId);
            return questions.Select(MapQuestionToDto).ToList();
        }

        public async Task<List<QuestionForPlayerDto>> GetQuestionsForPlayerAsync(int quizId)
        {
            var questions = await _questionRepository.GetByQuizIdAsync(quizId);
            return questions.Select(q => new QuestionForPlayerDto
            {
                Id = q.Id,
                Text = q.Text,
                QuestionType = q.QuestionType,
                Order = q.Order,
                Answers = q.Answers.Select(a => new AnswerOptionDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    Order = a.Order
                }).OrderBy(a => a.Order).ToList()
            }).ToList();
        }

        public async Task<QuizDto> CreateAsync(CreateQuizDto dto, int userId)
        {
            var quiz = new Quiz
            {
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Difficulty = dto.Difficulty,
                TimeLimit = dto.TimeLimit,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow,
                Questions = dto.Questions.Select(q => new Question
                {
                    Text = q.Text,
                    QuestionType = q.QuestionType,
                    DifficultyLevel = q.DifficultyLevel,
                    Order = q.Order,
                    Answers = q.Answers.Select(a => new Answer
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect,
                        Order = a.Order
                    }).ToList()
                }).ToList()
            };

            await _quizRepository.CreateAsync(quiz);

            // Reload with includes
            var created = await _quizRepository.GetByIdAsync(quiz.Id);
            return MapToDto(created!);
        }

        public async Task<QuizDto?> UpdateAsync(int id, UpdateQuizDto dto)
        {
            var quiz = await _quizRepository.GetByIdAsync(id);
            if (quiz == null) return null;

            if (dto.Title != null) quiz.Title = dto.Title;
            if (dto.Description != null) quiz.Description = dto.Description;
            if (dto.CategoryId.HasValue) quiz.CategoryId = dto.CategoryId.Value;
            if (dto.Difficulty != null) quiz.Difficulty = dto.Difficulty;
            if (dto.TimeLimit.HasValue) quiz.TimeLimit = dto.TimeLimit.Value;
            if (dto.IsActive.HasValue) quiz.IsActive = dto.IsActive.Value;

            await _quizRepository.UpdateAsync(quiz);
            var updated = await _quizRepository.GetByIdAsync(id);
            return MapToDto(updated!);
        }

        public async Task DeleteAsync(int id)
        {
            await _quizRepository.DeleteAsync(id);
        }

        private static QuizDto MapToDto(Quiz q) => new()
        {
            Id = q.Id,
            Title = q.Title,
            Description = q.Description,
            CategoryId = q.CategoryId,
            CategoryName = q.Category?.Name ?? "",
            Difficulty = q.Difficulty,
            TimeLimit = q.TimeLimit,
            QuestionCount = q.Questions?.Count ?? 0,
            IsActive = q.IsActive,
            CreatedAt = q.CreatedAt
        };

        private static QuestionDto MapQuestionToDto(Question q) => new()
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
