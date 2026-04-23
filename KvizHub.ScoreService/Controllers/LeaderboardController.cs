using Microsoft.AspNetCore.Mvc;
using KvizHub.ScoreService.Models.DTOs;
using KvizHub.ScoreService.Services;

namespace KvizHub.ScoreService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaderboardEntryDto>>> GetGlobal([FromQuery] int top = 20)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var leaderboard = await _leaderboardService.GetGlobalLeaderboardAsync(token, top);
            return Ok(leaderboard);
        }

        [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<List<QuizLeaderboardEntryDto>>> GetByQuiz(int quizId, [FromQuery] int top = 20)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var leaderboard = await _leaderboardService.GetQuizLeaderboardAsync(quizId, token, top);
            return Ok(leaderboard);
        }
    }
}
