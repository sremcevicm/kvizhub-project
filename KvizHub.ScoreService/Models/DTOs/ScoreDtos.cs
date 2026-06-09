namespace KvizHub.ScoreService.Models.DTOs
{
    // Submit attempt
    public class SubmitAttemptDto
    {
        public int QuizId { get; set; }
        public int TimeTakenSeconds { get; set; }
        public List<SubmitAnswerDto> Answers { get; set; } = new();
    }

        public class SubmitAnswerDto
    {
        public int QuestionId { get; set; }

        /// <summary>
        /// Selected answer ID for SingleChoice / TrueFalse.
        /// </summary>
        public int SelectedAnswerId { get; set; }

        /// <summary>
        /// Selected answer IDs for MultipleChoice questions.
        /// </summary>
        public List<int> SelectedAnswerIds { get; set; } = new();

        /// <summary>
        /// Text answer for FillInBlank questions.
        /// </summary>
        public string? TextAnswer { get; set; }
    }

    // Attempt result
    public class AttemptResultDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int TimeTakenSeconds { get; set; }
        public double Percentage { get; set; }
        public DateTime CompletedAt { get; set; }
        public List<AttemptAnswerResultDto> Answers { get; set; } = new();
    }

    public class AttemptAnswerResultDto
    {
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
        /// <summary>
        /// Selected answer IDs for MultipleChoice.
        /// </summary>
        public List<int> SelectedAnswerIds { get; set; } = new();
        /// <summary>
        /// Text answer for FillInBlank.
        /// </summary>
        public string? TextAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }

    // Leaderboard
    public class LeaderboardEntryDto
    {
        public int Rank { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int TotalScore { get; set; }
        public int QuizzesCompleted { get; set; }
        public double AveragePercentage { get; set; }
    }

    public class QuizLeaderboardEntryDto
    {
        public int Rank { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int BestScore { get; set; }
        public int TimeTakenSeconds { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    // User stats
    public class UserStatsDto
    {
        public int UserId { get; set; }
        public int TotalAttempts { get; set; }
        public int TotalScore { get; set; }
        public double AveragePercentage { get; set; }
        public int BestScore { get; set; }
        public List<AttemptResultDto> RecentAttempts { get; set; } = new();
    }

        // Internal DTOs for inter-service communication
    public class QuestionWithAnswersDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Question type: SingleChoice, MultipleChoice, TrueFalse, FillInBlank
        /// </summary>
        public string QuestionType { get; set; } = "SingleChoice";
        public List<AnswerCheckDto> Answers { get; set; } = new();
    }

    public class AnswerCheckDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }

    public class UserInfoDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
