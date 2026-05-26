import api from './api';

const authService = {
  register: async (username, email, password) => {
    const response = await api.post('/api/auth/register', { username, email, password });
    return response.data;
  },

  login: async (usernameOrEmail, password) => {
    const response = await api.post('/api/auth/login', { usernameOrEmail, password });
    if (response.data.accessToken) {
      localStorage.setItem('token', response.data.accessToken);
      localStorage.setItem('refreshToken', response.data.refreshToken);
      localStorage.setItem('user', JSON.stringify({
        id: response.data.userId,
        username: response.data.username,
        email: response.data.email,
        role: response.data.role,
      }));
    }
    return response.data;
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
    return response.data;
  },

  updateProfile: async (data) => {
    const response = await api.put('/api/users/me', data);
    return response.data;
  },
};

export default authService;
