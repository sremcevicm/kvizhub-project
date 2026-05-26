import React, { createContext, useContext, useState, useEffect } from 'react';
import authService from '../services/authService';

const AuthContext = createContext(null);

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const savedUser = authService.getCurrentUser();
    if (savedUser) {
      setUser(savedUser);
    }
    setLoading(false);
  }, []);

  const login = async (usernameOrEmail, password) => {
    const data = await authService.login(usernameOrEmail, password);
    const userData = { id: data.userId, username: data.username, email: data.email, role: data.role };
    setUser(userData);
    return data;
  };

  const register = async (username, email, password) => {
    const data = await authService.register(username, email, password);
    if (data.accessToken) {
      localStorage.setItem('token', data.accessToken);
      localStorage.setItem('refreshToken', data.refreshToken);
      localStorage.setItem('user', JSON.stringify({
        id: data.userId,
        username: data.username,
        email: data.email,
        role: data.role,
      }));
      setUser({ id: data.userId, username: data.username, email: data.email, role: data.role });
    }
    return data;
  };

  const logout = () => {
    authService.logout();
    setUser(null);
  };

  const isAdmin = () => user?.role === 'Admin';

  const value = {
    user,
    loading,
    login,
    register,
    logout,
    isAdmin,
    isAuthenticated: !!user,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export default AuthContext;
