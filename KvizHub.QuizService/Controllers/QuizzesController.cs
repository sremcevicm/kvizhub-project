using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KvizHub.QuizService.Models.DTOs;
using KvizHub.QuizService.Services;

namespace KvizHub.QuizService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizServiceLogic _quizService;

        public QuizzesController(IQuizServiceLogic quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<ActionResult<List<QuizDto>>> GetAll()
        {
            var quizzes = await _quizService.GetAllAsync();
            return Ok(quizzes);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<QuizDto>>> GetFiltered(
            [FromQuery] int? categoryId,
            [FromQuery] string? difficulty,
            [FromQuery] string? search)
        {
            var quizzes = await _quizService.GetFilteredAsync(categoryId, difficulty, search);
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDto>> GetById(int id)
        {
            var quiz = await _quizService.GetByIdAsync(id);
            if (quiz == null) return NotFound();
            return Ok(quiz);
        }

        [HttpGet("{id}/questions")]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestions(int id)
        {
            var questions = await _quizService.GetQuestionsWithAnswersAsync(id);
            return Ok(questions);
        }

        [HttpGet("{id}/play")]
        public async Task<ActionResult<List<QuestionForPlayerDto>>> GetQuestionsForPlayer(int id)
        {
            var questions = await _quizService.GetQuestionsForPlayerAsync(id);
            return Ok(questions);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<QuizDto>> Create([FromBody] CreateQuizDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var quiz = await _quizService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = quiz.Id }, quiz);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<QuizDto>> Update(int id, [FromBody] UpdateQuizDto dto)
        {
            var quiz = await _quizService.UpdateAsync(id, dto);
            if (quiz == null) return NotFound();
            return Ok(quiz);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _quizService.DeleteAsync(id);
            return NoContent();
        }
    }
}
