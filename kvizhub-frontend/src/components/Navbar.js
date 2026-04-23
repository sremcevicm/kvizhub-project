import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Navbar.css';

const Navbar = () => {
  const { user, isAuthenticated, isAdmin, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <Link to="/">🧠 KvizHub</Link>
      </div>
      <div className="navbar-links">
        <Link to="/">Početna</Link>
        <Link to="/quizzes">Kvizovi</Link>
        <Link to="/leaderboard">Rang lista</Link>
        {isAuthenticated && (
          <>
            <Link to="/my-stats">Moji rezultati</Link>
            {isAdmin() && <Link to="/admin" className="admin-link">Admin</Link>}
          </>
        )}
      </div>
      <div className="navbar-auth">
        {isAuthenticated ? (
          <>
            <span className="username">👤 {user.username}</span>
            <button onClick={handleLogout} className="btn-logout">Odjavi se</button>
          </>
        ) : (
          <>
            <Link to="/login" className="btn-login">Prijava</Link>
            <Link to="/register" className="btn-register">Registracija</Link>
          </>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
