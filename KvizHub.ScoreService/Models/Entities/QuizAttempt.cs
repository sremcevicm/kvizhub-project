namespace KvizHub.ScoreService.Models.Entities
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int TimeTakenSeconds { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }

        public List<AttemptAnswer> Answers { get; set; } = new();
    }
}
