import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Home.css';

const Home = () => {
  const { isAuthenticated } = useAuth();

  return (
    <div className="home">
      <div className="hero">
        <h1>🧠 KvizHub</h1>
        <p className="subtitle">Platforma za testiranje znanja sa rang listom</p>
        <p className="description">
          Testirajte svoje znanje iz raznih oblasti, takmičite se sa drugim igračima
          i osvajajte mesto na rang listi!
        </p>
        <div className="hero-actions">
          {isAuthenticated ? (
            <Link to="/quizzes" className="btn btn-primary">Započni kviz</Link>
          ) : (
            <>
              <Link to="/register" className="btn btn-primary">Registruj se</Link>
              <Link to="/login" className="btn btn-secondary">Prijavi se</Link>
            </>
          )}
        </div>
      </div>

      <div className="features">
        <div className="feature-card">
          <span className="feature-icon">📚</span>
          <h3>Raznovrsni kvizovi</h3>
          <p>Programiranje, istorija, nauka i opšte znanje</p>
        </div>
        <div className="feature-card">
          <span className="feature-icon">🏆</span>
          <h3>Rang lista</h3>
          <p>Takmičite se i pratite svoj napredak</p>
        </div>
        <div className="feature-card">
          <span className="feature-icon">⏱️</span>
          <h3>Vremenski izazov</h3>
          <p>Rešavajte kvizove u zadatom roku</p>
        </div>
      </div>
    </div>
  );
};

export default Home;
