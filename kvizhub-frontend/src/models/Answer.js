/**
 * Answer model – represents a single answer option in a question
 */
export class Answer {
  constructor({ id, text, isCorrect = false, order = 0 } = {}) {
    this.id = id;
    this.text = text || '';
    this.isCorrect = isCorrect;
    this.order = order;
  }
}

/**
 * Answer sent by the user when submitting an attempt
 * Supports different question types:
 * - SingleChoice/TrueFalse: selectedAnswerId
 * - MultipleChoice: selectedAnswerIds[]
 * - FillInBlank: textAnswer
 */
export class SubmitAnswer {
  constructor({ questionId, selectedAnswerId = 0, selectedAnswerIds = [], textAnswer = '' } = {}) {
    this.questionId = questionId;
    this.selectedAnswerId = selectedAnswerId;
    this.selectedAnswerIds = selectedAnswerIds;
    this.textAnswer = textAnswer;
  }
}

export default Answer;
