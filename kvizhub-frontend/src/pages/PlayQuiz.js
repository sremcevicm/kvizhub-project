import React, { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import quizService from '../services/quizService';
import scoreService from '../services/scoreService';
import { useAuth } from '../context/AuthContext';
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

  const handleSubmit = useCallback(async () => {
    if (submitting) return;
    setSubmitting(true);

    const timeTaken = quiz.timeLimitSeconds - timeLeft;
    const answerList = Object.entries(answers).map(([questionId, answerId]) => ({
      questionId: parseInt(questionId),
      selectedAnswerId: answerId,
    }));

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
  }, [submitting, quiz, timeLeft, answers, id]);

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

  const selectAnswer = (questionId, answerId) => {
    setAnswers((prev) => ({ ...prev, [questionId]: answerId }));
  };

  const formatTime = (seconds) => {
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return `${m}:${s.toString().padStart(2, '0')}`;
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
        <h3>{currentQuestion?.text}</h3>
        <div className="answers">
          {currentQuestion?.answers.map((answer) => (
            <button
              key={answer.id}
              className={`answer-btn ${answers[currentQuestion.id] === answer.id ? 'selected' : ''}`}
              onClick={() => selectAnswer(currentQuestion.id, answer.id)}
            >
              {answer.text}
            </button>
          ))}
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
            className={`dot ${i === currentIndex ? 'active' : ''} ${answers[q.id] ? 'answered' : ''}`}
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
