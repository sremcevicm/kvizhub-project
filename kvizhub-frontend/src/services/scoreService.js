import api from './api';

const scoreService = {
  submitAttempt: async (data) => {
    const response = await api.post('/api/attempts', data);
    return response.data;
  },

  getAttemptById: async (id) => {
    const response = await api.get(`/api/attempts/${id}`);
    return response.data;
  },

  getMyAttempts: async () => {
    const response = await api.get('/api/attempts/my');
    return response.data;
  },

  getMyStats: async () => {
    const response = await api.get('/api/attempts/my/stats');
    return response.data;
  },

  getGlobalLeaderboard: async (top = 20) => {
    const response = await api.get(`/api/leaderboard?top=${top}`);
    return response.data;
  },

  getQuizLeaderboard: async (quizId, top = 20) => {
    const response = await api.get(`/api/leaderboard/quiz/${quizId}?top=${top}`);
    return response.data;
  },
};

export default scoreService;
