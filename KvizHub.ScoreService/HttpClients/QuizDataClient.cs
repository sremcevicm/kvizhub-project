using System.Net.Http.Headers;
using KvizHub.ScoreService.Models.DTOs;

namespace KvizHub.ScoreService.HttpClients
{
    public interface IQuizDataClient
    {
        Task<List<QuestionWithAnswersDto>> GetQuestionsWithAnswersAsync(int quizId, string accessToken);
    }

    public class QuizDataClient : IQuizDataClient
    {
        private readonly HttpClient _httpClient;

        public QuizDataClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<QuestionWithAnswersDto>> GetQuestionsWithAnswersAsync(int quizId, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"/api/quizzes/{quizId}/questions");
            response.EnsureSuccessStatusCode();

            var questions = await response.Content.ReadFromJsonAsync<List<QuestionWithAnswersDto>>();
            return questions ?? new List<QuestionWithAnswersDto>();
        }
    }
}
