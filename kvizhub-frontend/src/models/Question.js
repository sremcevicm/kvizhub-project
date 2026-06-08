import Answer from './Answer';

/**
 * Supported question types
 */
export const QuestionType = {
  SINGLE_CHOICE: 'SingleChoice',
  MULTIPLE_CHOICE: 'MultipleChoice',
  TRUE_FALSE: 'TrueFalse',
  FILL_IN_BLANK: 'FillInBlank'
};

/**
 * Human-readable labels for question types
 */
export const QuestionTypeLabel = {
  [QuestionType.SINGLE_CHOICE]: 'Jedan tačan odgovor',
  [QuestionType.MULTIPLE_CHOICE]: 'Više tačnih odgovora',
  [QuestionType.TRUE_FALSE]: 'Tačno/Netačno',
  [QuestionType.FILL_IN_BLANK]: 'Unos teksta'
};

/**
 * Question model – represents a single question in a quiz
 */
export class Question {
  constructor({
    id,
    quizId,
    text,
    questionType = QuestionType.SINGLE_CHOICE,
    difficultyLevel = 'Medium',
    order = 0,
    answers = []
  } = {}) {
    this.id = id;
    this.quizId = quizId;
    this.text = text || '';
    this.questionType = questionType;
    this.difficultyLevel = difficultyLevel;
    this.order = order;
    this.answers = Array.isArray(answers) ? answers.map(a => new Answer(a)) : [];
  }

  get typeLabel() {
    return QuestionTypeLabel[this.questionType] || this.questionType;
  }

  get isSingleChoice() {
    return this.questionType === QuestionType.SINGLE_CHOICE;
  }

  get isMultipleChoice() {
    return this.questionType === QuestionType.MULTIPLE_CHOICE;
  }

  get isTrueFalse() {
    return this.questionType === QuestionType.TRUE_FALSE;
  }

  get isFillInBlank() {
    return this.questionType === QuestionType.FILL_IN_BLANK;
  }

  get correctAnswers() {
    return this.answers.filter(a => a.isCorrect);
  }

  get correctAnswerIds() {
    return this.correctAnswers.map(a => a.id);
  }

  get correctAnswerText() {
    if (this.isFillInBlank && this.correctAnswers.length > 0) {
      return this.correctAnswers[0].text;
    }
    return '';
  }
}

/**
 * DTO for creating/updating a question (admin)
 */
export class QuestionDto {
  constructor({ text, questionType = QuestionType.SINGLE_CHOICE, difficultyLevel = 'Medium', order = 0, answers = [] } = {}) {
    this.text = text;
    this.questionType = questionType;
    this.difficultyLevel = difficultyLevel;
    this.order = order;
    this.answers = answers;
  }
}

export default Question;
