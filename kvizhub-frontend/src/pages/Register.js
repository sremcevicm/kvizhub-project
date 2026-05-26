import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Auth.css';

const Register = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (password !== confirmPassword) {
      setError('Lozinke se ne poklapaju.');
      return;
    }

    setLoading(true);

    try {
      await register(username, email, password);
      navigate('/');
    } catch (err) {
      const data = err.response?.data;
      if (data?.errors) {
        // ASP.NET model validation errors
        const msgs = Object.values(data.errors).flat();
        setError(msgs.join(' '));
      } else {
        setError(data?.message || 'Greška pri registraciji.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h2>Registracija</h2>
        {error && <div className="error-message">{error}</div>}
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Korisničko ime</label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
              placeholder="Unesite korisničko ime"
            />
          </div>
          <div className="form-group">
            <label>Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              placeholder="Unesite email"
            />
          </div>
          <div className="form-group">
            <label>Lozinka</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              placeholder="Unesite lozinku"
              minLength={6}
            />
          </div>
          <div className="form-group">
            <label>Potvrda lozinke</label>
            <input
              type="password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
              placeholder="Ponovite lozinku"
            />
          </div>
          <button type="submit" className="btn-submit" disabled={loading}>
            {loading ? 'Registracija...' : 'Registruj se'}
          </button>
        </form>
        <p className="auth-link">
          Već imate nalog? <Link to="/login">Prijavite se</Link>
        </p>
      </div>
    </div>
  );
};

export default Register;
