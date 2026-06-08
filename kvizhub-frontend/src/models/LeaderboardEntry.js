/**
 * LeaderboardEntry model – represents a single entry in the global leaderboard
 */
export class LeaderboardEntry {
  constructor({
    rank,
    userId,
    username,
    totalScore = 0,
    quizzesCompleted = 0,
    averagePercentage = 0
  } = {}) {
    this.rank = rank;
    this.userId = userId;
    this.username = username || '';
    this.totalScore = totalScore;
    this.quizzesCompleted = quizzesCompleted;
    this.averagePercentage = averagePercentage;
  }

  get medal() {
    if (this.rank === 1) return '🥇';
    if (this.rank === 2) return '🥈';
    if (this.rank === 3) return '🥉';
    return null;
  }

  get formattedPercentage() {
    return `${this.averagePercentage}%`;
  }
}

/**
 * QuizLeaderboardEntry model – represents a single entry in a quiz-specific leaderboard
 */
export class QuizLeaderboardEntry {
  constructor({
    rank,
    userId,
    username,
    bestScore = 0,
    timeTakenSeconds = 0,
    completedAt
  } = {}) {
    this.rank = rank;
    this.userId = userId;
    this.username = username || '';
    this.bestScore = bestScore;
    this.timeTakenSeconds = timeTakenSeconds;
    this.completedAt = completedAt ? new Date(completedAt) : null;
  }

  get medal() {
    if (this.rank === 1) return '🥇';
    if (this.rank === 2) return '🥈';
    if (this.rank === 3) return '🥉';
    return null;
  }

  get timeFormatted() {
    const m = Math.floor(this.timeTakenSeconds / 60);
    const s = this.timeTakenSeconds % 60;
    return `${m}:${s.toString().padStart(2, '0')}`;
  }

  get formattedScore() {
    return `${this.bestScore}%`;
  }
}

export default LeaderboardEntry;
