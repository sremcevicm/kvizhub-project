using KvizHub.ScoreService.HttpClients;
using KvizHub.ScoreService.Models.DTOs;
using KvizHub.ScoreService.Repositories;

namespace KvizHub.ScoreService.Services
{
    public interface ILeaderboardService
    {
        Task<List<LeaderboardEntryDto>> GetGlobalLeaderboardAsync(string accessToken, int top = 20);
        Task<List<QuizLeaderboardEntryDto>> GetQuizLeaderboardAsync(int quizId, string accessToken, int top = 20);
    }

    public class LeaderboardService : ILeaderboardService
    {
        private readonly IAttemptRepository _attemptRepository;
        private readonly IUserDataClient _userDataClient;

        public LeaderboardService(IAttemptRepository attemptRepository, IUserDataClient userDataClient)
        {
            _attemptRepository = attemptRepository;
            _userDataClient = userDataClient;
        }

        public async Task<List<LeaderboardEntryDto>> GetGlobalLeaderboardAsync(string accessToken, int top = 20)
        {
            var allAttempts = await _attemptRepository.GetAllAsync();

            // Get all users for username lookup
            var users = await _userDataClient.GetAllUsersAsync(accessToken);
            var userMap = users.ToDictionary(u => u.Id, u => u.Username);

            var grouped = allAttempts
                .GroupBy(a => a.UserId)
                .Select(g => new LeaderboardEntryDto
                {
                    UserId = g.Key,
                    Username = userMap.GetValueOrDefault(g.Key, "Unknown"),
                    TotalScore = g.Sum(a => a.Score),
                    QuizzesCompleted = g.Count(),
                    AveragePercentage = Math.Round(
                        g.Average(a => (double)a.CorrectAnswers / Math.Max(a.TotalQuestions, 1) * 100), 1)
                })
                .OrderByDescending(e => e.TotalScore)
                .Take(top)
                .ToList();

            for (int i = 0; i < grouped.Count; i++)
            {
                grouped[i].Rank = i + 1;
            }

            return grouped;
        }

        public async Task<List<QuizLeaderboardEntryDto>> GetQuizLeaderboardAsync(int quizId, string accessToken, int top = 20)
        {
            var quizAttempts = await _attemptRepository.GetByQuizIdAsync(quizId);

            var users = await _userDataClient.GetAllUsersAsync(accessToken);
            var userMap = users.ToDictionary(u => u.Id, u => u.Username);

            // Best attempt per user for this quiz
            var grouped = quizAttempts
                .GroupBy(a => a.UserId)
                .Select(g =>
                {
                    var best = g.OrderByDescending(a => a.Score).ThenBy(a => a.TimeTakenSeconds).First();
                    return new QuizLeaderboardEntryDto
                    {
                        UserId = g.Key,
                        Username = userMap.GetValueOrDefault(g.Key, "Unknown"),
                        BestScore = best.Score,
                        TimeTakenSeconds = best.TimeTakenSeconds,
                        CompletedAt = best.CompletedAt
                    };
                })
                .OrderByDescending(e => e.BestScore)
                .ThenBy(e => e.TimeTakenSeconds)
                .Take(top)
                .ToList();

            for (int i = 0; i < grouped.Count; i++)
            {
                grouped[i].Rank = i + 1;
            }

            return grouped;
        }
    }
}
