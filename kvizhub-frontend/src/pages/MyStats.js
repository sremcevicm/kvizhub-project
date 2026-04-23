import React, { useState, useEffect } from 'react';
import scoreService from '../services/scoreService';
import './MyStats.css';

const MyStats = () => {
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadStats();
  }, []);

  const loadStats = async () => {
    try {
      const data = await scoreService.getMyStats();
      setStats(data);
    } catch (err) {
      console.error('Error loading stats:', err);
    } finally {
      setLoading(false);
    }
  };

  const formatTime = (seconds) => {
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return `${m}:${s.toString().padStart(2, '0')}`;
  };

  if (loading) return <div className="my-stats"><div className="loading">Učitavanje...</div></div>;

  return (
    <div className="my-stats">
      <h1>📊 Moji rezultati</h1>

      {stats && (
        <>
          <div className="stats-grid">
            <div className="stat-card">
              <div className="stat-value">{stats.totalAttempts}</div>
              <div className="stat-label">Ukupno pokušaja</div>
            </div>
            <div className="stat-card">
              <div className="stat-value">{stats.totalScore}</div>
              <div className="stat-label">Ukupni bodovi</div>
            </div>
            <div className="stat-card">
              <div className="stat-value">{stats.averagePercentage}%</div>
              <div className="stat-label">Prosečan rezultat</div>
            </div>
            <div className="stat-card">
              <div className="stat-value">{stats.bestScore}%</div>
              <div className="stat-label">Najbolji rezultat</div>
            </div>
          </div>

          <h2>Poslednji kvizovi</h2>
          {stats.recentAttempts.length === 0 ? (
            <p className="no-attempts">Još niste odigrali nijedan kviz.</p>
          ) : (
            <div className="attempts-list">
              {stats.recentAttempts.map((attempt) => (
                <div key={attempt.id} className="attempt-row">
                  <div className="attempt-info">
                    <span className="attempt-quiz">Kviz #{attempt.quizId}</span>
                    <span className="attempt-date">
                      {new Date(attempt.completedAt).toLocaleDateString('sr-Latn')}
                    </span>
                  </div>
                  <div className="attempt-result">
                    <span className="attempt-score">{attempt.percentage}%</span>
                    <span className="attempt-details">
                      {attempt.correctAnswers}/{attempt.totalQuestions} · {formatTime(attempt.timeTakenSeconds)}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default MyStats;
