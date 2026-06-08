/**
 * Attempt model – represents a quiz attempt with results
 */
export class AttemptResult {
  constructor({
    id,
    userId,
    quizId,
    score = 0,
    totalQuestions = 0,
    correctAnswers = 0,
    timeTakenSeconds = 0,
    percentage = 0,
    completedAt,
    answers = []
  } = {}) {
    this.id = id;
    this.userId = userId;
    this.quizId = quizId;
    this.score = score;
    this.totalQuestions = totalQuestions;
    this.correctAnswers = correctAnswers;
    this.timeTakenSeconds = timeTakenSeconds;
    this.percentage = percentage;
    this.completedAt = completedAt ? new Date(completedAt) : null;
    this.answers = Array.isArray(answers) ? answers : [];
  }

  get timeFormatted() {
    const m = Math.floor(this.timeTakenSeconds / 60);
    const s = this.timeTakenSeconds % 60;
    return `${m}:${s.toString().padStart(2, '0')}`;
  }

  get dateFormatted() {
    if (!this.completedAt) return '';
    return this.completedAt.toLocaleDateString('sr-Latn');
  }

  get isPerfect() {
    return this.percentage === 100;
  }

  get isPassing() {
    return this.percentage >= 50;
  }
}

/**
 * Attempt answer model – represents a single answer within an attempt
 */
export class AttemptAnswer {
  constructor({ questionId, selectedAnswerId, isCorrect = false, selectedAnswerIds = [], textAnswer = '' } = {}) {
    this.questionId = questionId;
    this.selectedAnswerId = selectedAnswerId;
    this.isCorrect = isCorrect;
    this.selectedAnswerIds = Array.isArray(selectedAnswerIds) ? selectedAnswerIds : [];
    this.textAnswer = textAnswer || '';
  }
}

/**
 * User stats model – aggregated statistics for a user
 */
export class UserStats {
  constructor({
    userId,
    totalAttempts = 0,
    totalScore = 0,
    averagePercentage = 0,
    bestScore = 0,
    recentAttempts = []
  } = {}) {
    this.userId = userId;
    this.totalAttempts = totalAttempts;
    this.totalScore = totalScore;
    this.averagePercentage = averagePercentage;
    this.bestScore = bestScore;
    this.recentAttempts = Array.isArray(recentAttempts)
      ? recentAttempts.map(a => new AttemptResult(a))
      : [];
  }
}

export default AttemptResult;
