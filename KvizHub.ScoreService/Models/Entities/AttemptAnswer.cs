namespace KvizHub.ScoreService.Models.Entities
{
    public class AttemptAnswer
    {
        public int Id { get; set; }
        public int QuizAttemptId { get; set; }
        public int QuestionId { get; set; }

        /// <summary>
        /// Selected answer ID for SingleChoice / TrueFalse.
        /// </summary>
        public int SelectedAnswerId { get; set; }

        /// <summary>
        /// Comma-separated selected answer IDs for MultipleChoice.
        /// </summary>
        public string? SelectedAnswerIdsCsv { get; set; }

        /// <summary>
        /// Text answer for FillInBlank questions.
        /// </summary>
        public string? TextAnswer { get; set; }

        public bool IsCorrect { get; set; }

        public QuizAttempt QuizAttempt { get; set; } = null!;
    }
}
