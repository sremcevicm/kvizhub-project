import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import Navbar from './components/Navbar';
import ProtectedRoute from './components/ProtectedRoute';
import AdminRoute from './components/AdminRoute';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import QuizList from './pages/QuizList';
import PlayQuiz from './pages/PlayQuiz';
import Leaderboard from './pages/Leaderboard';
import MyStats from './pages/MyStats';
import Admin from './pages/Admin';
import './App.css';

function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="App">
          <Navbar />
          <main className="main-content">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/quizzes" element={<QuizList />} />
              <Route path="/quiz/:id" element={<PlayQuiz />} />
              <Route path="/leaderboard" element={<Leaderboard />} />
              <Route path="/leaderboard/quiz/:quizId" element={<Leaderboard />} />
              <Route path="/my-stats" element={
                <ProtectedRoute><MyStats /></ProtectedRoute>
              } />
              <Route path="/admin" element={
                <AdminRoute><Admin /></AdminRoute>
              } />
            </Routes>
          </main>
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;
