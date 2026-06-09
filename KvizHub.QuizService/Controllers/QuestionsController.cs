using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KvizHub.QuizService.Models.DTOs;
using KvizHub.QuizService.Services;

namespace KvizHub.QuizService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

                [HttpGet("quiz/{quizId}")]
        public async Task<ActionResult<List<QuestionDto>>> GetByQuizId(int quizId)
        {
            var questions = await _questionService.GetByQuizIdAsync(quizId);
            return Ok(questions);
        }

        [HttpPost("quiz/{quizId}")]
        public async Task<ActionResult<QuestionDto>> Create(int quizId, [FromBody] CreateQuestionDto dto)
        {
            var question = await _questionService.CreateAsync(quizId, dto);
            return CreatedAtAction(nameof(GetById), new { id = question.Id }, question);
        }

                [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<QuestionDto>> GetById(int id)
        {
            var question = await _questionService.GetByIdAsync(id);
            if (question == null) return NotFound();
            return Ok(question);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<QuestionDto>> Update(int id, [FromBody] UpdateQuestionDto dto)
        {
            var question = await _questionService.UpdateAsync(id, dto);
            if (question == null) return NotFound();
            return Ok(question);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _questionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
