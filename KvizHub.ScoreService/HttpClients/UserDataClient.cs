using System.Net.Http.Headers;
using KvizHub.ScoreService.Models.DTOs;

namespace KvizHub.ScoreService.HttpClients
{
    public interface IUserDataClient
    {
        Task<List<UserInfoDto>> GetAllUsersAsync(string accessToken);
        Task<UserInfoDto?> GetUserByIdAsync(int userId, string accessToken);
    }

    public class UserDataClient : IUserDataClient
    {
        private readonly HttpClient _httpClient;

        public UserDataClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserInfoDto>> GetAllUsersAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("/api/users");
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<UserInfoDto>>();
            return users ?? new List<UserInfoDto>();
        }

        public async Task<UserInfoDto?> GetUserByIdAsync(int userId, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"/api/users/{userId}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserInfoDto>();
        }
    }
}
