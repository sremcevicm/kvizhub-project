namespace KvizHub.QuizService.Models.DTOs
{
    // Category DTOs
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int QuizCount { get; set; }
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    // Quiz DTOs
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int TimeLimit { get; set; }
        public int QuestionCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateQuizDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string Difficulty { get; set; } = "Medium";
        // Accept both "timeLimit" and "timeLimitMinutes" from frontend
        private int _timeLimit = 30;
        public int TimeLimit
        {
            get => _timeLimit;
            set => _timeLimit = value;
        }
        public int TimeLimitMinutes
        {
            get => _timeLimit;
            set => _timeLimit = value;
        }
        public List<CreateQuestionDto> Questions { get; set; } = new();
    }

    public class UpdateQuizDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? Difficulty { get; set; }
        public int? TimeLimit { get; set; }
        public bool? IsActive { get; set; }
    }

    // Question DTOs
    public class QuestionDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } = string.Empty;
        public int Order { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();
    }

    public class CreateQuestionDto
    {
        public string Text { get; set; } = string.Empty;
        public string QuestionType { get; set; } = "SingleChoice";
        public string DifficultyLevel { get; set; } = "Medium";
        public int Order { get; set; }
        public List<CreateAnswerDto> Answers { get; set; } = new();
    }

    public class UpdateQuestionDto
    {
        public string? Text { get; set; }
        public string? QuestionType { get; set; }
        public string? DifficultyLevel { get; set; }
        public int? Order { get; set; }
        public List<CreateAnswerDto>? Answers { get; set; }
    }

    // Answer DTOs
    public class AnswerDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int Order { get; set; }
    }

    public class CreateAnswerDto
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int Order { get; set; }
    }

    // For quiz-taking: questions without correct answer flags
    public class QuestionForPlayerDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string QuestionType { get; set; } = string.Empty;
        public int Order { get; set; }
        public List<AnswerOptionDto> Answers { get; set; } = new();
    }

    public class AnswerOptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
