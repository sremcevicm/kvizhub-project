using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KvizHub.QuizService.Models.Entities
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; } = null!;

        [Required, MaxLength(500)]
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public int Order { get; set; }
    }
}
