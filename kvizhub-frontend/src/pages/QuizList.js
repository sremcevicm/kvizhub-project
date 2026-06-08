import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import quizService from '../services/quizService';
import './QuizList.css';

const difficultyColors = {
  Easy: '#4CAF50',
  Medium: '#FF9800',
  Hard: '#f44336'
};

const QuizList = () => {
  const [quizzes, setQuizzes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [categoryId, setCategoryId] = useState('');
  const [difficulty, setDifficulty] = useState('');
  const [search, setSearch] = useState('');
  const [categories, setCategories] = useState([]);

  const loadQuizzes = async () => {
    setLoading(true);
    setError(null);
    try {
      const items = categoryId || difficulty || search
        ? await quizService.getFiltered(categoryId, difficulty, search)
        : await quizService.getAll();
      setQuizzes(items);
    } catch (err) {
      console.error('Error loading quizzes:', err);
      setError('Greška pri učitavanju kvizova.');
    } finally {
      setLoading(false);
    }
  };

  const loadCategories = async () => {
    try {
      const items = await quizService.getCategories();
      setCategories(items);
    } catch (err) {
      console.error('Error loading categories:', err);
    }
  };

    useEffect(() => {
    loadCategories();
    loadQuizzes();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleFilter = (e) => {
    e.preventDefault();
    loadQuizzes();
  };

  const handleReset = () => {
    setCategoryId('');
    setDifficulty('');
    setSearch('');
    setTimeout(() => loadQuizzes(), 0);
  };

  return (
    <div className="quiz-list-page">
      <h1>Svi kvizovi</h1>

      <form className="filters" onSubmit={handleFilter}>
        <select
          value={categoryId}
          onChange={(e) => setCategoryId(e.target.value)}
        >
          <option value="">Sve kategorije</option>
          {categories.map((cat) => (
            <option key={cat.id} value={cat.id}>{cat.name}</option>
          ))}
        </select>
        <select
          value={difficulty}
          onChange={(e) => setDifficulty(e.target.value)}
        >
          <option value="">Svi nivoi</option>
          <option value="Easy">Lak</option>
          <option value="Medium">Srednji</option>
          <option value="Hard">Težak</option>
        </select>
        <input
          type="text"
          placeholder="Pretraži kvizove..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <button type="submit" className="btn-filter">Filtriraj</button>
        <button type="button" className="btn-reset" onClick={handleReset}>Reset</button>
      </form>

      {loading && <div className="loading">Učitavanje kvizova...</div>}
      {error && <div className="no-results">{error}</div>}

      {!loading && !error && quizzes.length === 0 && (
        <div className="no-results">Nema dostupnih kvizova.</div>
      )}

      <div className="quiz-grid">
        {quizzes.map((quiz) => (
          <div key={quiz.id} className="quiz-card">
            <div className="quiz-card-header">
              <h3>{quiz.title}</h3>
              <span
                className="badge"
                style={{ backgroundColor: difficultyColors[quiz.difficulty] || '#888' }}
              >
                {quiz.difficultyLabel}
              </span>
            </div>
            <p className="quiz-description">{quiz.description}</p>
            <div className="quiz-meta">
              <span>📁 {quiz.categoryName}</span>
              <span>❓ {quiz.questionCount} pitanja</span>
              <span>⏱️ {quiz.timeLimit} min</span>
            </div>
            <Link to={`/quiz/${quiz.id}`} className="btn-play">
              Igraj kviz
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
};

export default QuizList;
