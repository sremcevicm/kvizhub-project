import React, { useState, useEffect } from 'react';
import quizService from '../services/quizService';
import categoryService from '../services/categoryService';
import './Admin.css';

const Admin = () => {
  const [activeTab, setActiveTab] = useState('quizzes');

  // Categories state
  const [categories, setCategories] = useState([]);
  const [newCategory, setNewCategory] = useState('');

  // Quizzes state
  const [quizzes, setQuizzes] = useState([]);
  const [showQuizForm, setShowQuizForm] = useState(false);
  const [quizForm, setQuizForm] = useState({
    title: '', description: '', categoryId: '', difficulty: 'Easy', timeLimitMinutes: 10,
  });

  // Questions state
  const [selectedQuizId, setSelectedQuizId] = useState(null);
  const [questions, setQuestions] = useState([]);

  useEffect(() => {
    loadCategories();
    loadQuizzes();
  }, []);

  const loadCategories = async () => {
    try {
      const data = await categoryService.getAll();
      setCategories(data);
    } catch (err) { console.error(err); }
  };

  const loadQuizzes = async () => {
    try {
      const data = await quizService.getAll();
      setQuizzes(data);
    } catch (err) { console.error(err); }
  };

  const loadQuestions = async (quizId) => {
    try {
      const data = await quizService.getQuestionsWithAnswers(quizId);
      setQuestions(data);
      setSelectedQuizId(quizId);
    } catch (err) { console.error(err); }
  };

  // Category CRUD
  const handleAddCategory = async () => {
    if (!newCategory.trim()) return;
    try {
      await categoryService.create({ name: newCategory.trim() });
      setNewCategory('');
      loadCategories();
    } catch (err) { console.error(err); }
  };

  const handleDeleteCategory = async (id) => {
    if (!window.confirm('Obrisati kategoriju?')) return;
    try {
      await categoryService.delete(id);
      loadCategories();
    } catch (err) { console.error(err); }
  };

  // Quiz CRUD
  const handleCreateQuiz = async (e) => {
    e.preventDefault();
    try {
      await quizService.create({
        ...quizForm,
        categoryId: parseInt(quizForm.categoryId),
        timeLimitMinutes: parseInt(quizForm.timeLimitMinutes),
      });
      setShowQuizForm(false);
      setQuizForm({ title: '', description: '', categoryId: '', difficulty: 'Easy', timeLimitMinutes: 10 });
      loadQuizzes();
    } catch (err) { console.error(err); }
  };

  const handleDeleteQuiz = async (id) => {
    if (!window.confirm('Obrisati kviz?')) return;
    try {
      await quizService.delete(id);
      loadQuizzes();
    } catch (err) { console.error(err); }
  };

  return (
    <div className="admin-page">
      <h1>⚙️ Admin panel</h1>

      <div className="admin-tabs">
        <button
          className={`tab ${activeTab === 'quizzes' ? 'active' : ''}`}
          onClick={() => setActiveTab('quizzes')}
        >
          Kvizovi
        </button>
        <button
          className={`tab ${activeTab === 'categories' ? 'active' : ''}`}
          onClick={() => setActiveTab('categories')}
        >
          Kategorije
        </button>
        <button
          className={`tab ${activeTab === 'questions' ? 'active' : ''}`}
          onClick={() => setActiveTab('questions')}
        >
          Pitanja
        </button>
      </div>

      {/* Categories Tab */}
      {activeTab === 'categories' && (
        <div className="admin-section">
          <h2>Kategorije</h2>
          <div className="add-form">
            <input
              type="text"
              value={newCategory}
              onChange={(e) => setNewCategory(e.target.value)}
              placeholder="Naziv nove kategorije"
            />
            <button onClick={handleAddCategory} className="btn-add">Dodaj</button>
          </div>
          <div className="list">
            {categories.map((c) => (
              <div key={c.id} className="list-item">
                <span>{c.name}</span>
                <button onClick={() => handleDeleteCategory(c.id)} className="btn-delete">Obriši</button>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Quizzes Tab */}
      {activeTab === 'quizzes' && (
        <div className="admin-section">
          <div className="section-header">
            <h2>Kvizovi</h2>
            <button onClick={() => setShowQuizForm(!showQuizForm)} className="btn-add">
              {showQuizForm ? 'Otkaži' : '+ Novi kviz'}
            </button>
          </div>

          {showQuizForm && (
            <form onSubmit={handleCreateQuiz} className="quiz-form">
              <input
                type="text" placeholder="Naslov kviza" required
                value={quizForm.title}
                onChange={(e) => setQuizForm({ ...quizForm, title: e.target.value })}
              />
              <textarea
                placeholder="Opis kviza"
                value={quizForm.description}
                onChange={(e) => setQuizForm({ ...quizForm, description: e.target.value })}
              />
              <select
                value={quizForm.categoryId} required
                onChange={(e) => setQuizForm({ ...quizForm, categoryId: e.target.value })}
              >
                <option value="">Izaberite kategoriju</option>
                {categories.map((c) => (
                  <option key={c.id} value={c.id}>{c.name}</option>
                ))}
              </select>
              <select
                value={quizForm.difficulty}
                onChange={(e) => setQuizForm({ ...quizForm, difficulty: e.target.value })}
              >
                <option value="Easy">Lako</option>
                <option value="Medium">Srednje</option>
                <option value="Hard">Teško</option>
              </select>
              <input
                type="number" placeholder="Vremenski limit (min)" min="1"
                value={quizForm.timeLimitMinutes}
                onChange={(e) => setQuizForm({ ...quizForm, timeLimitMinutes: e.target.value })}
              />
              <button type="submit" className="btn-submit">Kreiraj kviz</button>
            </form>
          )}

          <div className="list">
            {quizzes.map((q) => (
              <div key={q.id} className="list-item quiz-item">
                <div>
                  <strong>{q.title}</strong>
                  <span className="quiz-meta-admin">{q.categoryName} · {q.difficulty} · {q.questionCount} pitanja</span>
                </div>
                <div className="item-actions">
                  <button onClick={() => { loadQuestions(q.id); setActiveTab('questions'); }} className="btn-edit">
                    Pitanja
                  </button>
                  <button onClick={() => handleDeleteQuiz(q.id)} className="btn-delete">Obriši</button>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Questions Tab */}
      {activeTab === 'questions' && (
        <div className="admin-section">
          <h2>Pitanja {selectedQuizId ? `(Kviz #${selectedQuizId})` : ''}</h2>
          {!selectedQuizId ? (
            <p>Izaberite kviz iz tab-a "Kvizovi" da biste videli pitanja.</p>
          ) : (
            <>
              <div className="list">
                {questions.length === 0 ? (
                  <p className="no-items">Nema pitanja za ovaj kviz.</p>
                ) : (
                  questions.map((q, i) => (
                    <div key={q.id} className="list-item question-item">
                      <div>
                        <strong>{i + 1}. {q.text}</strong>
                        <div className="answers-list">
                          {q.answers.map((a) => (
                            <span key={a.id} className={`answer-tag ${a.isCorrect ? 'correct' : ''}`}>
                              {a.text} {a.isCorrect ? '✓' : ''}
                            </span>
                          ))}
                        </div>
                      </div>
                    </div>
                  ))
                )}
              </div>
            </>
          )}
        </div>
      )}
    </div>
  );
};

export default Admin;
