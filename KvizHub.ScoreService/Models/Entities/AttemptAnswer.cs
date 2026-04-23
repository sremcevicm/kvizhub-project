namespace KvizHub.ScoreService.Models.Entities
{
    public class AttemptAnswer
    {
        public int Id { get; set; }
        public int QuizAttemptId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
        public bool IsCorrect { get; set; }

        public QuizAttempt QuizAttempt { get; set; } = null!;
    }
}
