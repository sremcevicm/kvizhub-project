import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import quizService from '../services/quizService';
import categoryService from '../services/categoryService';
import './QuizList.css';

const QuizList = () => {
  const [quizzes, setQuizzes] = useState([]);
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState('');
  const [selectedDifficulty, setSelectedDifficulty] = useState('');
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadCategories();
    loadQuizzes();
  }, []);

  const loadCategories = async () => {
    try {
      const data = await categoryService.getAll();
      setCategories(data);
    } catch (err) {
      console.error('Error loading categories:', err);
    }
  };

  const loadQuizzes = async () => {
    setLoading(true);
    try {
      const data = await quizService.getAll();
      setQuizzes(data);
    } catch (err) {
      console.error('Error loading quizzes:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleFilter = async () => {
    setLoading(true);
    try {
      const data = await quizService.getFiltered(
        selectedCategory || null,
        selectedDifficulty || null,
        search || null
      );
      setQuizzes(data);
    } catch (err) {
      console.error('Error filtering quizzes:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleReset = () => {
    setSelectedCategory('');
    setSelectedDifficulty('');
    setSearch('');
    loadQuizzes();
  };

  const getDifficultyBadge = (difficulty) => {
    const colors = {
      Easy: '#4caf50',
      Medium: '#ff9800',
      Hard: '#f44336',
    };
    return (
      <span className="badge" style={{ background: colors[difficulty] || '#999' }}>
        {difficulty === 'Easy' ? 'Lako' : difficulty === 'Medium' ? 'Srednje' : 'Teško'}
      </span>
    );
  };

  return (
    <div className="quiz-list-page">
      <h1>Kvizovi</h1>

      <div className="filters">
        <select value={selectedCategory} onChange={(e) => setSelectedCategory(e.target.value)}>
          <option value="">Sve kategorije</option>
          {categories.map((c) => (
            <option key={c.id} value={c.id}>{c.name}</option>
          ))}
        </select>

        <select value={selectedDifficulty} onChange={(e) => setSelectedDifficulty(e.target.value)}>
          <option value="">Sve težine</option>
          <option value="Easy">Lako</option>
          <option value="Medium">Srednje</option>
          <option value="Hard">Teško</option>
        </select>

        <input
          type="text"
          placeholder="Pretraži kvizove..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />

        <button onClick={handleFilter} className="btn-filter">Filtriraj</button>
        <button onClick={handleReset} className="btn-reset">Resetuj</button>
      </div>

      {loading ? (
        <div className="loading">Učitavanje kvizova...</div>
      ) : quizzes.length === 0 ? (
        <div className="no-results">Nema pronađenih kvizova.</div>
      ) : (
        <div className="quiz-grid">
          {quizzes.map((quiz) => (
            <div key={quiz.id} className="quiz-card">
              <div className="quiz-card-header">
                <h3>{quiz.title}</h3>
                {getDifficultyBadge(quiz.difficulty)}
              </div>
              <p className="quiz-description">{quiz.description}</p>
              <div className="quiz-meta">
                <span>📁 {quiz.categoryName}</span>
                <span>❓ {quiz.questionCount} pitanja</span>
                <span>⏱️ {quiz.timeLimitMinutes} min</span>
              </div>
              <Link to={`/quiz/${quiz.id}`} className="btn-play">Igraj</Link>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default QuizList;
