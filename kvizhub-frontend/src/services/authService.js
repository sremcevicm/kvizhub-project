import api from './api';
import { User, LoginResponse } from '../models/User';

const authService = {
  register: async (username, email, password) => {
    const response = await api.post('/api/auth/register', { username, email, password });
    return new LoginResponse(response.data);
  },

  login: async (usernameOrEmail, password) => {
    const response = await api.post('/api/auth/login', { usernameOrEmail, password });
    const data = new LoginResponse(response.data);
    if (data.accessToken) {
      localStorage.setItem('token', data.accessToken);
      localStorage.setItem('refreshToken', data.refreshToken);
      localStorage.setItem('user', JSON.stringify({
        id: data.userId,
        username: data.username,
        email: data.email,
        role: data.role,
      }));
    }
    return data;
  },

  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
  },

  getCurrentUser: () => {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  },

  getToken: () => localStorage.getItem('token'),

  getProfile: async () => {
    const response = await api.get('/api/users/me');
    return new User(response.data);
  },

  updateProfile: async (data) => {
    const response = await api.put('/api/users/me', data);
    return new User(response.data);
  },
};

export default authService;
