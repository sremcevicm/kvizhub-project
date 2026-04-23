import React, { useState, useEffect, useCallback } from 'react';
import { useParams, Link } from 'react-router-dom';
import scoreService from '../services/scoreService';
import './Leaderboard.css';

const Leaderboard = () => {
  const { quizId } = useParams();
  const [entries, setEntries] = useState([]);
  const [loading, setLoading] = useState(true);

  const loadLeaderboard = useCallback(async () => {
    setLoading(true);
    try {
      if (quizId) {
        const data = await scoreService.getQuizLeaderboard(quizId);
        setEntries(data);
      } else {
        const data = await scoreService.getGlobalLeaderboard();
        setEntries(data);
      }
    } catch (err) {
      console.error('Error loading leaderboard:', err);
    } finally {
      setLoading(false);
    }
  }, [quizId]);

  useEffect(() => {
    loadLeaderboard();
  }, [loadLeaderboard]);

  const getMedalEmoji = (rank) => {
    if (rank === 1) return '🥇';
    if (rank === 2) return '🥈';
    if (rank === 3) return '🥉';
    return rank;
  };

  return (
    <div className="leaderboard-page">
      <h1>🏆 {quizId ? 'Rang lista kviza' : 'Globalna rang lista'}</h1>

      {!quizId && (
        <p className="leaderboard-subtitle">Najbolji igrači na platformi</p>
      )}

      {loading ? (
        <div className="loading">Učitavanje rang liste...</div>
      ) : entries.length === 0 ? (
        <div className="no-results">Nema podataka za rang listu. Budite prvi!</div>
      ) : (
        <div className="leaderboard-table">
          <div className="table-header">
            <span className="col-rank">Rang</span>
            <span className="col-name">Igrač</span>
            {quizId ? (
              <>
                <span className="col-score">Najbolji rezultat</span>
                <span className="col-time">Vreme</span>
              </>
            ) : (
              <>
                <span className="col-score">Ukupni bodovi</span>
                <span className="col-quizzes">Kvizova</span>
                <span className="col-avg">Prosek</span>
              </>
            )}
          </div>

          {entries.map((entry) => (
            <div key={entry.userId} className={`table-row ${entry.rank <= 3 ? 'top-three' : ''}`}>
              <span className="col-rank">{getMedalEmoji(entry.rank)}</span>
              <span className="col-name">{entry.username}</span>
              {quizId ? (
                <>
                  <span className="col-score">{entry.bestScore}%</span>
                  <span className="col-time">
                    {Math.floor(entry.timeTakenSeconds / 60)}:{(entry.timeTakenSeconds % 60).toString().padStart(2, '0')}
                  </span>
                </>
              ) : (
                <>
                  <span className="col-score">{entry.totalScore}</span>
                  <span className="col-quizzes">{entry.quizzesCompleted}</span>
                  <span className="col-avg">{entry.averagePercentage}%</span>
                </>
              )}
            </div>
          ))}
        </div>
      )}

      <div className="leaderboard-actions">
        <Link to="/quizzes" className="btn-back-quizzes">← Nazad na kvizove</Link>
      </div>
    </div>
  );
};

export default Leaderboard;
