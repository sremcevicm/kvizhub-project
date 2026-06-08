import api from './api';
import AttemptResult, { UserStats } from '../models/Attempt';
import { LeaderboardEntry, QuizLeaderboardEntry } from '../models/LeaderboardEntry';

const mapAttemptList = (data) => {
  if (!data) return [];
  const items = data?.value || data || [];
  return Array.isArray(items) ? items.map(a => new AttemptResult(a)) : [];
};

const mapLeaderboard = (data) => {
  const items = data?.value || data || [];
  return Array.isArray(items) ? items.map(e => new LeaderboardEntry(e)) : [];
};

const mapQuizLeaderboard = (data) => {
  const items = data?.value || data || [];
  return Array.isArray(items) ? items.map(e => new QuizLeaderboardEntry(e)) : [];
};

const scoreService = {
  submitAttempt: async (data) => {
    const response = await api.post('/api/attempts', data);
    return new AttemptResult(response.data);
  },

  getAttemptById: async (id) => {
    const response = await api.get(`/api/attempts/${id}`);
    return new AttemptResult(response.data);
  },

  getMyAttempts: async () => {
    const response = await api.get('/api/attempts/my');
    return mapAttemptList(response.data);
  },

  getMyStats: async () => {
    const response = await api.get('/api/attempts/my/stats');
    return new UserStats(response.data);
  },

  getGlobalLeaderboard: async (top = 20) => {
    const response = await api.get(`/api/leaderboard?top=${top}`);
    return mapLeaderboard(response.data);
  },

  getQuizLeaderboard: async (quizId, top = 20) => {
    const response = await api.get(`/api/leaderboard/quiz/${quizId}?top=${top}`);
    return mapQuizLeaderboard(response.data);
  },
};

export default scoreService;
