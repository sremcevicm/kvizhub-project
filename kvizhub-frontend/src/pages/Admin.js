import React, { useState, useEffect } from 'react';
import quizService from '../services/quizService';
import categoryService from '../services/categoryService';
import './Admin.css';

const EMPTY_QUIZ_FORM = {
  title: '', description: '', categoryId: '', difficulty: 'Easy', timeLimitMinutes: 10,
};

const EMPTY_QUESTION_FORM = {
  text: '',
  questionType: 'SingleChoice',
  difficultyLevel: 'Medium',
  order: 1,
  answers: [
    { text: '', isCorrect: false },
    { text: '', isCorrect: false },
    { text: '', isCorrect: false },
    { text: '', isCorrect: false },
  ],
};

const Admin = () => {
  const [activeTab, setActiveTab] = useState('quizzes');

  // Categories state
  const [categories, setCategories] = useState([]);
  const [newCategory, setNewCategory] = useState('');
  const [editingCategory, setEditingCategory] = useState(null); // { id, name }

  // Quizzes state
  const [quizzes, setQuizzes] = useState([]);
  const [showQuizForm, setShowQuizForm] = useState(false);
  const [editingQuiz, setEditingQuiz] = useState(null); // null = create mode, object = edit mode
  const [quizForm, setQuizForm] = useState(EMPTY_QUIZ_FORM);

  // Questions state
  const [selectedQuiz, setSelectedQuiz] = useState(null); // full quiz object
  const [questions, setQuestions] = useState([]);
  const [showQuestionForm, setShowQuestionForm] = useState(false);
  const [editingQuestion, setEditingQuestion] = useState(null);
  const [questionForm, setQuestionForm] = useState(EMPTY_QUESTION_FORM);

  useEffect(() => {
    loadCategories();
    loadQuizzes();
  }, []);

  const loadCategories = async () => {
    try { setCategories(await categoryService.getAll()); }
    catch (err) { console.error(err); }
  };

  const loadQuizzes = async () => {
    try { setQuizzes(await quizService.getAll()); }
    catch (err) { console.error(err); }
  };

  const loadQuestions = async (quiz) => {
    try {
      const data = await quizService.getQuestionsWithAnswers(quiz.id);
      setQuestions(data);
      setSelectedQuiz(quiz);
      setShowQuestionForm(false);
      setEditingQuestion(null);
      setQuestionForm(EMPTY_QUESTION_FORM);
    } catch (err) { console.error(err); }
  };

  // ─── Category CRUD ────────────────────────────────────────────────────────
  const handleAddCategory = async () => {
    if (!newCategory.trim()) return;
    try {
      await categoryService.create({ name: newCategory.trim() });
      setNewCategory('');
      loadCategories();
    } catch (err) { console.error(err); }
  };

  const handleSaveEditCategory = async () => {
    if (!editingCategory?.name.trim()) return;
    try {
      await categoryService.update(editingCategory.id, { name: editingCategory.name.trim() });
      setEditingCategory(null);
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

  // ─── Quiz CRUD ────────────────────────────────────────────────────────────
  const openCreateQuiz = () => {
    setEditingQuiz(null);
    setQuizForm(EMPTY_QUIZ_FORM);
    setShowQuizForm(true);
  };

  const openEditQuiz = (q) => {
    setEditingQuiz(q);
    setQuizForm({
      title: q.title,
      description: q.description || '',
      categoryId: String(q.categoryId),
      difficulty: q.difficulty,
      timeLimitMinutes: q.timeLimit,
    });
    setShowQuizForm(true);
  };

  const handleSaveQuiz = async (e) => {
    e.preventDefault();
    const payload = {
      title: quizForm.title,
      description: quizForm.description,
      categoryId: parseInt(quizForm.categoryId),
      difficulty: quizForm.difficulty,
      timeLimitMinutes: parseInt(quizForm.timeLimitMinutes),
    };
    try {
      if (editingQuiz) {
        await quizService.update(editingQuiz.id, payload);
      } else {
        await quizService.create(payload);
      }
      setShowQuizForm(false);
      setEditingQuiz(null);
      setQuizForm(EMPTY_QUIZ_FORM);
      loadQuizzes();
    } catch (err) { console.error(err); }
  };

  const handleDeleteQuiz = async (id) => {
    if (!window.confirm('Obrisati kviz i sva pitanja?')) return;
    try {
      await quizService.delete(id);
      if (selectedQuiz?.id === id) setSelectedQuiz(null);
      loadQuizzes();
    } catch (err) { console.error(err); }
  };

  // ─── Question CRUD ────────────────────────────────────────────────────────
  const openCreateQuestion = () => {
    setEditingQuestion(null);
    const nextOrder = questions.length + 1;
    setQuestionForm({ ...EMPTY_QUESTION_FORM, order: nextOrder, answers: getDefaultAnswers('SingleChoice') });
    setShowQuestionForm(true);
  };

  const openEditQuestion = (q) => {
    setEditingQuestion(q);
    setQuestionForm({
      text: q.text,
      questionType: q.questionType,
      difficultyLevel: q.difficultyLevel,
      order: q.order,
      answers: q.answers.map(a => ({ id: a.id, text: a.text, isCorrect: a.isCorrect })),
    });
    setShowQuestionForm(true);
  };

  const getDefaultAnswers = (type) => {
    if (type === 'TrueFalse') return [{ text: 'Tačno', isCorrect: true }, { text: 'Netačno', isCorrect: false }];
    if (type === 'FillInBlank') return [{ text: '', isCorrect: true }];
    return [
      { text: '', isCorrect: false }, { text: '', isCorrect: false },
      { text: '', isCorrect: false }, { text: '', isCorrect: false },
    ];
  };

  const handleQuestionTypeChange = (type) => {
    setQuestionForm(f => ({ ...f, questionType: type, answers: getDefaultAnswers(type) }));
  };

  const updateAnswer = (idx, field, value) => {
    setQuestionForm(f => {
      const answers = f.answers.map((a, i) => i === idx ? { ...a, [field]: value } : a);
      // SingleChoice/TrueFalse: only one correct
      if (field === 'isCorrect' && value === true && (f.questionType === 'SingleChoice' || f.questionType === 'TrueFalse')) {
        return { ...f, answers: answers.map((a, i) => ({ ...a, isCorrect: i === idx })) };
      }
      return { ...f, answers };
    });
  };

  const handleSaveQuestion = async (e) => {
    e.preventDefault();
    const payload = {
      text: questionForm.text,
      questionType: questionForm.questionType,
      difficultyLevel: questionForm.difficultyLevel,
      order: parseInt(questionForm.order),
      answers: questionForm.answers.filter(a => a.text.trim() !== '').map(a => ({
        text: a.text.trim(),
        isCorrect: a.isCorrect,
      })),
    };
    try {
      if (editingQuestion) {
        await quizService.updateQuestion(editingQuestion.id, payload);
      } else {
        await quizService.createQuestion(selectedQuiz.id, payload);
      }
      setShowQuestionForm(false);
      setEditingQuestion(null);
      setQuestionForm(EMPTY_QUESTION_FORM);
      loadQuestions(selectedQuiz);
    } catch (err) { console.error(err); }
  };

  const handleDeleteQuestion = async (id) => {
    if (!window.confirm('Obrisati pitanje?')) return;
    try {
      await quizService.deleteQuestion(id);
      loadQuestions(selectedQuiz);
    } catch (err) { console.error(err); }
  };

  // ─── Render ───────────────────────────────────────────────────────────────
  return (
    <div className="admin-page">
      <h1>⚙️ Admin panel</h1>

      <div className="admin-tabs">
        {['quizzes', 'categories', 'questions'].map(tab => (
          <button key={tab}
            className={`tab ${activeTab === tab ? 'active' : ''}`}
            onClick={() => setActiveTab(tab)}
          >
            {tab === 'quizzes' ? 'Kvizovi' : tab === 'categories' ? 'Kategorije' : 'Pitanja'}
          </button>
        ))}
      </div>

      {/* ── Categories Tab ── */}
      {activeTab === 'categories' && (
        <div className="admin-section">
          <h2>Kategorije</h2>
          <div className="add-form">
            <input type="text" value={newCategory}
              onChange={(e) => setNewCategory(e.target.value)}
              placeholder="Naziv nove kategorije"
              onKeyDown={(e) => e.key === 'Enter' && handleAddCategory()}
            />
            <button onClick={handleAddCategory} className="btn-add">Dodaj</button>
          </div>
          <div className="list">
            {categories.map((c) => (
              <div key={c.id} className="list-item">
                {editingCategory?.id === c.id ? (
                  <>
                    <input className="inline-edit" value={editingCategory.name}
                      onChange={(e) => setEditingCategory({ ...editingCategory, name: e.target.value })}
                      onKeyDown={(e) => e.key === 'Enter' && handleSaveEditCategory()}
                      autoFocus
                    />
                    <div className="item-actions">
                      <button onClick={handleSaveEditCategory} className="btn-add">Sačuvaj</button>
                      <button onClick={() => setEditingCategory(null)} className="btn-cancel">Otkaži</button>
                    </div>
                  </>
                ) : (
                  <>
                    <span>{c.name} <small className="quiz-meta-admin">({c.quizCount} kvizova)</small></span>
                    <div className="item-actions">
                      <button onClick={() => setEditingCategory({ id: c.id, name: c.name })} className="btn-edit">Izmeni</button>
                      <button onClick={() => handleDeleteCategory(c.id)} className="btn-delete">Obriši</button>
                    </div>
                  </>
                )}
              </div>
            ))}
          </div>
        </div>
      )}

      {/* ── Quizzes Tab ── */}
      {activeTab === 'quizzes' && (
        <div className="admin-section">
          <div className="section-header">
            <h2>Kvizovi</h2>
            <button onClick={showQuizForm && !editingQuiz ? () => setShowQuizForm(false) : openCreateQuiz} className="btn-add">
              {showQuizForm && !editingQuiz ? 'Otkaži' : '+ Novi kviz'}
            </button>
          </div>

          {showQuizForm && (
            <form onSubmit={handleSaveQuiz} className="quiz-form">
              <h3 style={{ margin: 0, color: '#555' }}>{editingQuiz ? `Izmena: ${editingQuiz.title}` : 'Novi kviz'}</h3>
              <input type="text" placeholder="Naslov kviza" required
                value={quizForm.title}
                onChange={(e) => setQuizForm({ ...quizForm, title: e.target.value })}
              />
              <textarea placeholder="Opis kviza"
                value={quizForm.description}
                onChange={(e) => setQuizForm({ ...quizForm, description: e.target.value })}
              />
              <select value={quizForm.categoryId} required
                onChange={(e) => setQuizForm({ ...quizForm, categoryId: e.target.value })}
              >
                <option value="">Izaberite kategoriju</option>
                {categories.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
              </select>
              <select value={quizForm.difficulty}
                onChange={(e) => setQuizForm({ ...quizForm, difficulty: e.target.value })}
              >
                <option value="Easy">Lako</option>
                <option value="Medium">Srednje</option>
                <option value="Hard">Teško</option>
              </select>
              <input type="number" placeholder="Vremenski limit (min)" min="1"
                value={quizForm.timeLimitMinutes}
                onChange={(e) => setQuizForm({ ...quizForm, timeLimitMinutes: e.target.value })}
              />
              <div style={{ display: 'flex', gap: '0.75rem' }}>
                <button type="submit" className="btn-submit" style={{ flex: 1 }}>
                  {editingQuiz ? 'Sačuvaj izmene' : 'Kreiraj kviz'}
                </button>
                <button type="button" className="btn-cancel"
                  onClick={() => { setShowQuizForm(false); setEditingQuiz(null); setQuizForm(EMPTY_QUIZ_FORM); }}
                >
                  Otkaži
                </button>
              </div>
            </form>
          )}

          <div className="list">
            {quizzes.map((q) => (
              <div key={q.id} className="list-item quiz-item">
                <div>
                  <strong>{q.title}</strong>
                  <span className="quiz-meta-admin">{q.categoryName} · {q.difficulty} · {q.questionCount} pitanja · {q.timeLimit} min</span>
                </div>
                <div className="item-actions">
                  <button onClick={() => { loadQuestions(q); setActiveTab('questions'); }} className="btn-edit">
                    Pitanja
                  </button>
                  <button onClick={() => { openEditQuiz(q); setActiveTab('quizzes'); }} className="btn-edit">
                    Izmeni
                  </button>
                  <button onClick={() => handleDeleteQuiz(q.id)} className="btn-delete">Obriši</button>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* ── Questions Tab ── */}
      {activeTab === 'questions' && (
        <div className="admin-section">
          <div className="section-header">
            <h2>
              Pitanja {selectedQuiz ? <span className="quiz-meta-admin" style={{ display: 'inline', marginLeft: '0.5rem' }}>— {selectedQuiz.title}</span> : ''}
            </h2>
            {selectedQuiz && (
              <button onClick={showQuestionForm && !editingQuestion ? () => setShowQuestionForm(false) : openCreateQuestion} className="btn-add">
                {showQuestionForm && !editingQuestion ? 'Otkaži' : '+ Novo pitanje'}
              </button>
            )}
          </div>

          {!selectedQuiz ? (
            <p className="no-items">Izaberite kviz iz taba "Kvizovi" → kliknite "Pitanja".</p>
          ) : (
            <>
              {/* Question Form */}
              {showQuestionForm && (
                <form onSubmit={handleSaveQuestion} className="quiz-form">
                  <h3 style={{ margin: 0, color: '#555' }}>{editingQuestion ? 'Izmena pitanja' : 'Novo pitanje'}</h3>

                  <textarea placeholder="Tekst pitanja" required
                    value={questionForm.text}
                    onChange={(e) => setQuestionForm(f => ({ ...f, text: e.target.value }))}
                  />

                  <div className="form-row">
                    <select value={questionForm.questionType}
                      onChange={(e) => handleQuestionTypeChange(e.target.value)}
                    >
                      <option value="SingleChoice">Jedan tačan odgovor</option>
                      <option value="MultipleChoice">Više tačnih odgovora</option>
                      <option value="TrueFalse">Tačno/Netačno</option>
                      <option value="FillInBlank">Popuni prazninu</option>
                    </select>
                    <select value={questionForm.difficultyLevel}
                      onChange={(e) => setQuestionForm(f => ({ ...f, difficultyLevel: e.target.value }))}
                    >
                      <option value="Easy">Lako</option>
                      <option value="Medium">Srednje</option>
                      <option value="Hard">Teško</option>
                    </select>
                    <input type="number" placeholder="Redosled" min="1"
                      value={questionForm.order}
                      onChange={(e) => setQuestionForm(f => ({ ...f, order: e.target.value }))}
                      style={{ width: '90px' }}
                    />
                  </div>

                  {/* Answers */}
                  <div className="answers-form">
                    <label style={{ fontWeight: 600, color: '#555' }}>Odgovori:</label>
                    {questionForm.questionType === 'FillInBlank' ? (
                      <div className="answer-row">
                        <input type="text" placeholder="Tačan odgovor" required
                          value={questionForm.answers[0]?.text || ''}
                          onChange={(e) => updateAnswer(0, 'text', e.target.value)}
                        />
                        <span className="correct-badge">✓ Tačan</span>
                      </div>
                    ) : questionForm.questionType === 'TrueFalse' ? (
                      questionForm.answers.map((a, i) => (
                        <div key={i} className="answer-row">
                          <span className="answer-label">{a.text}</span>
                          <label className="correct-toggle">
                            <input type="radio" name="correct" checked={a.isCorrect}
                              onChange={() => updateAnswer(i, 'isCorrect', true)}
                            /> Tačan
                          </label>
                        </div>
                      ))
                    ) : (
                      questionForm.answers.map((a, i) => (
                        <div key={i} className="answer-row">
                          <input type="text" placeholder={`Odgovor ${i + 1}`}
                            value={a.text}
                            onChange={(e) => updateAnswer(i, 'text', e.target.value)}
                          />
                          <label className="correct-toggle">
                            {questionForm.questionType === 'MultipleChoice' ? (
                              <input type="checkbox" checked={a.isCorrect}
                                onChange={(e) => updateAnswer(i, 'isCorrect', e.target.checked)}
                              />
                            ) : (
                              <input type="radio" name="correct" checked={a.isCorrect}
                                onChange={() => updateAnswer(i, 'isCorrect', true)}
                              />
                            )}
                            Tačan
                          </label>
                        </div>
                      ))
                    )}
                  </div>

                  <div style={{ display: 'flex', gap: '0.75rem' }}>
                    <button type="submit" className="btn-submit" style={{ flex: 1 }}>
                      {editingQuestion ? 'Sačuvaj izmene' : 'Dodaj pitanje'}
                    </button>
                    <button type="button" className="btn-cancel"
                      onClick={() => { setShowQuestionForm(false); setEditingQuestion(null); }}
                    >
                      Otkaži
                    </button>
                  </div>
                </form>
              )}

              {/* Questions List */}
              <div className="list">
                {questions.length === 0 ? (
                  <p className="no-items">Nema pitanja. Dodajte prvo pitanje.</p>
                ) : (
                  questions.map((q, i) => (
                    <div key={q.id} className="list-item question-item">
                      <div style={{ flex: 1 }}>
                        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                          <strong>{i + 1}. {q.text}</strong>
                          <div className="item-actions">
                            <button onClick={() => openEditQuestion(q)} className="btn-edit">Izmeni</button>
                            <button onClick={() => handleDeleteQuestion(q.id)} className="btn-delete">Obriši</button>
                          </div>
                        </div>
                        <div style={{ fontSize: '0.8rem', color: '#888', margin: '0.25rem 0' }}>
                          {q.questionType} · {q.difficultyLevel}
                        </div>
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
