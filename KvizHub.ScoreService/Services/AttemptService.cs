using KvizHub.ScoreService.HttpClients;
using KvizHub.ScoreService.Models.DTOs;
using KvizHub.ScoreService.Models.Entities;
using KvizHub.ScoreService.Repositories;

namespace KvizHub.ScoreService.Services
{
    public interface IAttemptService
    {
        Task<AttemptResultDto> SubmitAttemptAsync(int userId, SubmitAttemptDto dto, string accessToken);
        Task<AttemptResultDto?> GetAttemptByIdAsync(int id);
        Task<UserStatsDto> GetUserStatsAsync(int userId);
        Task<List<AttemptResultDto>> GetUserAttemptsAsync(int userId);
    }

    public class AttemptService : IAttemptService
    {
        private readonly IAttemptRepository _attemptRepository;
        private readonly IQuizDataClient _quizDataClient;

        public AttemptService(IAttemptRepository attemptRepository, IQuizDataClient quizDataClient)
        {
            _attemptRepository = attemptRepository;
            _quizDataClient = quizDataClient;
        }

        public async Task<AttemptResultDto> SubmitAttemptAsync(int userId, SubmitAttemptDto dto, string accessToken)
        {
            // Get correct answers from QuizService (now includes QuestionType and answer text)
            var questions = await _quizDataClient.GetQuestionsWithAnswersAsync(dto.QuizId, accessToken);

            // Build lookup: questionId -> (questionType, correctAnswerIds, correctAnswerText)
            var questionLookup = questions.ToDictionary(
                q => q.Id,
                q => (
                    Type: q.QuestionType,
                    CorrectIds: q.Answers.Where(a => a.IsCorrect).Select(a => a.Id).ToHashSet(),
                    CorrectText: q.Answers.Where(a => a.IsCorrect).Select(a => a.Text).FirstOrDefault() ?? ""
                )
            );

            int correctCount = 0;
            var attemptAnswers = new List<AttemptAnswer>();

            foreach (var answer in dto.Answers)
            {
                bool isCorrect = EvaluateAnswer(answer, questionLookup);
                if (isCorrect) correctCount++;

                attemptAnswers.Add(new AttemptAnswer
                {
                    QuestionId = answer.QuestionId,
                    SelectedAnswerId = answer.SelectedAnswerId,
                    SelectedAnswerIdsCsv = answer.SelectedAnswerIds?.Count > 0
                        ? string.Join(",", answer.SelectedAnswerIds)
                        : null,
                    TextAnswer = answer.TextAnswer,
                    IsCorrect = isCorrect
                });
            }

            int totalQuestions = questions.Count;
            int score = totalQuestions > 0
                ? (int)Math.Round((double)correctCount / totalQuestions * 100)
                : 0;

            var attempt = new QuizAttempt
            {
                UserId = userId,
                QuizId = dto.QuizId,
                Score = score,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctCount,
                TimeTakenSeconds = dto.TimeTakenSeconds,
                StartedAt = DateTime.UtcNow.AddSeconds(-dto.TimeTakenSeconds),
                CompletedAt = DateTime.UtcNow,
                Answers = attemptAnswers
            };

            await _attemptRepository.CreateAsync(attempt);

            return MapToResultDto(attempt);
        }

        /// <summary>
        /// Evaluates whether the user's answer is correct based on question type.
        /// </summary>
        private static bool EvaluateAnswer(
            SubmitAnswerDto answer,
            Dictionary<int, (string Type, HashSet<int> CorrectIds, string CorrectText)> lookup)
        {
            if (!lookup.TryGetValue(answer.QuestionId, out var info))
                return false;

            return info.Type switch
            {
                // SingleChoice & TrueFalse: compare selectedAnswerId against the set of correct IDs
                "SingleChoice" or "TrueFalse" =>
                    info.CorrectIds.Contains(answer.SelectedAnswerId),

                // MultipleChoice: all selected IDs must match all correct IDs (order doesn't matter)
                "MultipleChoice" =>
                    info.CorrectIds.SetEquals(answer.SelectedAnswerIds ?? new List<int>()),

                // FillInBlank: case-insensitive text comparison, trimmed
                "FillInBlank" =>
                    !string.IsNullOrWhiteSpace(answer.TextAnswer) &&
                    string.Equals(
                        answer.TextAnswer.Trim(),
                        info.CorrectText.Trim(),
                        StringComparison.OrdinalIgnoreCase),

                // Unknown type – treat as incorrect
                _ => false
            };
        }

        public async Task<AttemptResultDto?> GetAttemptByIdAsync(int id)
        {
            var attempt = await _attemptRepository.GetByIdAsync(id);
            if (attempt == null) return null;
            return MapToResultDto(attempt);
        }

        public async Task<UserStatsDto> GetUserStatsAsync(int userId)
        {
            var attempts = await _attemptRepository.GetByUserIdAsync(userId);

            return new UserStatsDto
            {
                UserId = userId,
                TotalAttempts = attempts.Count,
                TotalScore = attempts.Sum(a => a.Score),
                AveragePercentage = attempts.Count > 0
                    ? Math.Round(attempts.Average(a => (double)a.CorrectAnswers / Math.Max(a.TotalQuestions, 1) * 100), 1)
                    : 0,
                BestScore = attempts.Count > 0 ? attempts.Max(a => a.Score) : 0,
                RecentAttempts = attempts.Take(10).Select(MapToResultDto).ToList()
            };
        }

        public async Task<List<AttemptResultDto>> GetUserAttemptsAsync(int userId)
        {
            var attempts = await _attemptRepository.GetByUserIdAsync(userId);
            return attempts.Select(MapToResultDto).ToList();
        }

        private static AttemptResultDto MapToResultDto(QuizAttempt attempt) => new()
        {
            Id = attempt.Id,
            UserId = attempt.UserId,
            QuizId = attempt.QuizId,
            Score = attempt.Score,
            TotalQuestions = attempt.TotalQuestions,
            CorrectAnswers = attempt.CorrectAnswers,
            TimeTakenSeconds = attempt.TimeTakenSeconds,
            Percentage = attempt.TotalQuestions > 0
                ? Math.Round((double)attempt.CorrectAnswers / attempt.TotalQuestions * 100, 1)
                : 0,
            CompletedAt = attempt.CompletedAt,
            Answers = attempt.Answers.Select(a => new AttemptAnswerResultDto
            {
                QuestionId = a.QuestionId,
                SelectedAnswerId = a.SelectedAnswerId,
                SelectedAnswerIds = !string.IsNullOrEmpty(a.SelectedAnswerIdsCsv)
                    ? a.SelectedAnswerIdsCsv.Split(',').Select(int.Parse).ToList()
                    : new List<int>(),
                TextAnswer = a.TextAnswer,
                IsCorrect = a.IsCorrect
            }).ToList()
        };
    }
}
