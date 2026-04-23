using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KvizHub.QuizService.Models.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuizId { get; set; }

        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; } = null!;

        [Required, MaxLength(1000)]
        public string Text { get; set; } = string.Empty;

        [Required, MaxLength(30)]
        public string QuestionType { get; set; } = "SingleChoice";
        // SingleChoice, MultipleChoice, TrueFalse, FillInBlank

        [MaxLength(20)]
        public string DifficultyLevel { get; set; } = "Medium"; // Easy, Medium, Hard

        public int Order { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
