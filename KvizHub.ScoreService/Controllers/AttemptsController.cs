using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KvizHub.ScoreService.Models.DTOs;
using KvizHub.ScoreService.Services;

namespace KvizHub.ScoreService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttemptsController : ControllerBase
    {
        private readonly IAttemptService _attemptService;

        public AttemptsController(IAttemptService attemptService)
        {
            _attemptService = attemptService;
        }

        [HttpPost]
        public async Task<ActionResult<AttemptResultDto>> Submit([FromBody] SubmitAttemptDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var result = await _attemptService.SubmitAttemptAsync(userId, dto, token);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttemptResultDto>> GetById(int id)
        {
            var result = await _attemptService.GetAttemptByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<AttemptResultDto>>> GetMyAttempts()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var attempts = await _attemptService.GetUserAttemptsAsync(userId);
            return Ok(attempts);
        }

        [HttpGet("my/stats")]
        public async Task<ActionResult<UserStatsDto>> GetMyStats()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var stats = await _attemptService.GetUserStatsAsync(userId);
            return Ok(stats);
        }

        [HttpGet("user/{userId}/stats")]
        public async Task<ActionResult<UserStatsDto>> GetUserStats(int userId)
        {
            var stats = await _attemptService.GetUserStatsAsync(userId);
            return Ok(stats);
        }
    }
}
