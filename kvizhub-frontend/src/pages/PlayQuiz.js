import React, { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import quizService from '../services/quizService';
import scoreService from '../services/scoreService';
import { useAuth } from '../context/AuthContext';
import { QuestionType } from '../models/Question';
import './PlayQuiz.css';

const PlayQuiz = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { isAuthenticated } = useAuth();

  const [quiz, setQuiz] = useState(null);
  const [questions, setQuestions] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [answers, setAnswers] = useState({});
  const [timeLeft, setTimeLeft] = useState(0);
  const [started, setStarted] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(true);

  const loadQuiz = useCallback(async () => {
    try {
      const quizData = await quizService.getById(id);
      setQuiz(quizData);
      setTimeLeft(quizData.timeLimitSeconds);
    } catch (err) {
      console.error('Error loading quiz:', err);
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    loadQuiz();
  }, [loadQuiz]);

  const startQuiz = async () => {
    if (!isAuthenticated) {
      navigate('/login');
      return;
    }
    try {
      const questionsData = await quizService.getQuestionsForPlayer(id);
      setQuestions(questionsData);
      setStarted(true);
    } catch (err) {
      console.error('Error loading questions:', err);
    }
  };

  // Build the answer payload for submitting
  const buildAnswerList = () => {
    return questions.map((q) => {
      const ans = answers[q.id] || {};
      const base = { questionId: q.id };

      switch (q.questionType) {
        case QuestionType.MULTIPLE_CHOICE:
          return {
            ...base,
            selectedAnswerId: 0,
            selectedAnswerIds: ans.selectedAnswerIds || [],
            textAnswer: ''
          };
        case QuestionType.FILL_IN_BLANK:
          return {
            ...base,
            selectedAnswerId: 0,
            selectedAnswerIds: [],
            textAnswer: ans.textAnswer || ''
          };
        case QuestionType.SINGLE_CHOICE:
        case QuestionType.TRUE_FALSE:
        default:
          return {
            ...base,
            selectedAnswerId: ans.selectedAnswerId || 0,
            selectedAnswerIds: [],
            textAnswer: ''
          };
      }
    });
  };

  const handleSubmit = useCallback(async () => {
    if (submitting) return;
    setSubmitting(true);

    const timeTaken = quiz.timeLimitSeconds - timeLeft;
    const answerList = buildAnswerList();

    try {
      const attemptResult = await scoreService.submitAttempt({
        quizId: parseInt(id),
        timeTakenSeconds: timeTaken,
        answers: answerList,
      });
      setResult(attemptResult);
    } catch (err) {
      console.error('Error submitting attempt:', err);
      alert('Greška pri slanju odgovora.');
    } finally {
      setSubmitting(false);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [submitting, quiz, timeLeft, id, answers, questions]);

  // Timer
  useEffect(() => {
    if (!started || result) return;

    const timer = setInterval(() => {
      setTimeLeft((prev) => {
        if (prev <= 1) {
          clearInterval(timer);
          handleSubmit();
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, [started, result, handleSubmit]);

  // ---- Answer handlers for each question type ----

  // SingleChoice / TrueFalse: select one answer
  const selectSingleAnswer = (questionId, answerId) => {
    setAnswers((prev) => ({
      ...prev,
      [questionId]: { ...prev[questionId], selectedAnswerId: answerId }
    }));
  };

  // MultipleChoice: toggle an answer in the set
  const toggleMultipleAnswer = (questionId, answerId) => {
    setAnswers((prev) => {
      const current = prev[questionId]?.selectedAnswerIds || [];
      const updated = current.includes(answerId)
        ? current.filter((id) => id !== answerId)
        : [...current, answerId];
      return { ...prev, [questionId]: { ...prev[questionId], selectedAnswerIds: updated } };
    });
  };

  // FillInBlank: store text input
  const setTextAnswer = (questionId, text) => {
    setAnswers((prev) => ({
      ...prev,
      [questionId]: { ...prev[questionId], textAnswer: text }
    }));
  };

  const formatTime = (seconds) => {
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return `${m}:${s.toString().padStart(2, '0')}`;
  };

  // ---- Render helpers ----

  const renderQuestionTypeLabel = (type) => {
    const labels = {
      [QuestionType.SINGLE_CHOICE]: 'Jedan tačan odgovor',
      [QuestionType.MULTIPLE_CHOICE]: 'Više tačnih odgovora',
      [QuestionType.TRUE_FALSE]: 'Tačno / Netačno',
      [QuestionType.FILL_IN_BLANK]: 'Unesite odgovor'
    };
    return labels[type] || type;
  };

  const renderAnswers = (question) => {
    const qId = question.id;
    const ansData = answers[qId] || {};

    switch (question.questionType) {
      // SingleChoice & TrueFalse: single-select buttons
      case QuestionType.SINGLE_CHOICE:
      case QuestionType.TRUE_FALSE:
        return question.answers.map((answer) => (
          <button
            key={answer.id}
            className={`answer-btn ${ansData.selectedAnswerId === answer.id ? 'selected' : ''}`}
            onClick={() => selectSingleAnswer(qId, answer.id)}
          >
            {answer.text}
          </button>
        ));

      // MultipleChoice: toggle buttons (checkbox style)
      case QuestionType.MULTIPLE_CHOICE:
        return question.answers.map((answer) => {
          const isSelected = (ansData.selectedAnswerIds || []).includes(answer.id);
          return (
            <button
              key={answer.id}
              className={`answer-btn answer-btn-multi ${isSelected ? 'selected' : ''}`}
              onClick={() => toggleMultipleAnswer(qId, answer.id)}
            >
              <span className="checkbox-indicator">{isSelected ? '☑' : '☐'}</span>
              {answer.text}
            </button>
          );
        });

      // FillInBlank: text input
      case QuestionType.FILL_IN_BLANK:
        return (
          <div className="fill-blank-input-wrap">
            <input
              type="text"
              className="fill-blank-input"
              placeholder="Upišite svoj odgovor..."
              value={ansData.textAnswer || ''}
              onChange={(e) => setTextAnswer(qId, e.target.value)}
              autoFocus
            />
          </div>
        );

      default:
        return <p className="unsupported-type">Nepodržan tip pitanja: {question.questionType}</p>;
    }
  };

  const isQuestionAnswered = (questionId) => {
    const a = answers[questionId];
    if (!a) return false;
    const q = questions.find((qq) => qq.id === questionId);
    if (!q) return false;

    switch (q.questionType) {
      case QuestionType.SINGLE_CHOICE:
      case QuestionType.TRUE_FALSE:
        return !!a.selectedAnswerId;
      case QuestionType.MULTIPLE_CHOICE:
        return (a.selectedAnswerIds || []).length > 0;
      case QuestionType.FILL_IN_BLANK:
        return !!(a.textAnswer && a.textAnswer.trim());
      default:
        return false;
    }
  };

  if (loading) return <div className="play-quiz"><div className="loading">Učitavanje...</div></div>;

  // Show result
  if (result) {
    return (
      <div className="play-quiz">
        <div className="result-card">
          <h2>Rezultat kviza</h2>
          <div className="result-score">{result.percentage}%</div>
          <div className="result-details">
            <p>Tačnih odgovora: <strong>{result.correctAnswers}/{result.totalQuestions}</strong></p>
            <p>Bodovi: <strong>{result.score}</strong></p>
            <p>Vreme: <strong>{result.timeFormatted}</strong></p>
          </div>
          <div className="result-actions">
            <button onClick={() => navigate('/quizzes')} className="btn-back">Nazad na kvizove</button>
            <button onClick={() => navigate(`/leaderboard/quiz/${id}`)} className="btn-leaderboard">
              Rang lista kviza
            </button>
          </div>
        </div>
      </div>
    );
  }

  // Show start screen
  if (!started) {
    return (
      <div className="play-quiz">
        <div className="start-card">
          <h2>{quiz?.title}</h2>
          <p>{quiz?.description}</p>
          <div className="quiz-info">
            <span>📁 {quiz?.categoryName}</span>
            <span>❓ {quiz?.questionCount} pitanja</span>
            <span>⏱️ {quiz?.timeLimit} minuta</span>
          </div>
          <button onClick={startQuiz} className="btn-start">Započni kviz</button>
        </div>
      </div>
    );
  }

  // Show questions
  const currentQuestion = questions[currentIndex];

  return (
    <div className="play-quiz">
      <div className="quiz-header">
        <span className="question-counter">
          Pitanje {currentIndex + 1} / {questions.length}
        </span>
        <span className={`timer ${timeLeft < 60 ? 'timer-warning' : ''}`}>
          ⏱️ {formatTime(timeLeft)}
        </span>
      </div>

      <div className="progress-bar">
        <div
          className="progress-fill"
          style={{ width: `${((currentIndex + 1) / questions.length) * 100}%` }}
        />
      </div>

      <div className="question-card">
        <div className="question-type-badge">
          {currentQuestion && renderQuestionTypeLabel(currentQuestion.questionType)}
        </div>
        <h3>{currentQuestion?.text}</h3>
        <div className="answers">
          {currentQuestion && renderAnswers(currentQuestion)}
        </div>
      </div>

      <div className="nav-buttons">
        <button
          onClick={() => setCurrentIndex((i) => Math.max(0, i - 1))}
          disabled={currentIndex === 0}
          className="btn-nav"
        >
          ← Prethodno
        </button>

        {currentIndex < questions.length - 1 ? (
          <button
            onClick={() => setCurrentIndex((i) => i + 1)}
            className="btn-nav"
          >
            Sledeće →
          </button>
        ) : (
          <button
            onClick={handleSubmit}
            disabled={submitting}
            className="btn-submit-quiz"
          >
            {submitting ? 'Slanje...' : 'Završi kviz'}
          </button>
        )}
      </div>

      <div className="question-dots">
        {questions.map((q, i) => (
          <button
            key={q.id}
            className={`dot ${i === currentIndex ? 'active' : ''} ${isQuestionAnswered(q.id) ? 'answered' : ''}`}
            onClick={() => setCurrentIndex(i)}
          >
            {i + 1}
          </button>
        ))}
      </div>
    </div>
  );
};

export default PlayQuiz;
