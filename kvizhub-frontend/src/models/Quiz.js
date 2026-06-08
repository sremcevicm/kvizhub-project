/**
 * Quiz model – represents a quiz with all its metadata
 */
export class Quiz {
  constructor({
    id,
    title,
    description,
    categoryId,
    categoryName,
    difficulty = 'Easy',
    timeLimit = 10,
    questionCount = 0,
    isActive = true,
    createdAt
  } = {}) {
    this.id = id;
    this.title = title || '';
    this.description = description || '';
    this.categoryId = categoryId;
    this.categoryName = categoryName || '';
    this.difficulty = difficulty;
    this.timeLimit = timeLimit;
    this.questionCount = questionCount;
    this.isActive = isActive;
    this.createdAt = createdAt ? new Date(createdAt) : null;
  }

  get difficultyLabel() {
    const labels = { Easy: 'Lak', Medium: 'Srednji', Hard: 'Težak' };
    return labels[this.difficulty] || this.difficulty;
  }

  get timeLimitSeconds() {
    return this.timeLimit * 60;
  }
}

/**
 * DTO for creating a new quiz (admin)
 */
export class CreateQuizData {
  constructor({ title, description, categoryId, difficulty = 'Easy', timeLimitMinutes = 10 } = {}) {
    this.title = title;
    this.description = description;
    this.categoryId = categoryId;
    this.difficulty = difficulty;
    this.timeLimitMinutes = timeLimitMinutes;
  }
}

export default Quiz;
