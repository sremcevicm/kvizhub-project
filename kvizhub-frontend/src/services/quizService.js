import api from './api';

const quizService = {
  getAll: async () => {
    const response = await api.get('/api/quizzes');
    return response.data;
  },

  getFiltered: async (categoryId, difficulty, search) => {
    const params = new URLSearchParams();
    if (categoryId) params.append('categoryId', categoryId);
    if (difficulty) params.append('difficulty', difficulty);
    if (search) params.append('search', search);
    const response = await api.get(`/api/quizzes/filter?${params.toString()}`);
    return response.data;
  },

  getById: async (id) => {
    const response = await api.get(`/api/quizzes/${id}`);
    return response.data;
  },

  getQuestionsForPlayer: async (id) => {
    const response = await api.get(`/api/quizzes/${id}/play`);
    return response.data;
  },

  getQuestionsWithAnswers: async (id) => {
    const response = await api.get(`/api/quizzes/${id}/questions`);
    return response.data;
  },

  create: async (data) => {
    const response = await api.post('/api/quizzes', data);
    return response.data;
  },

  update: async (id, data) => {
    const response = await api.put(`/api/quizzes/${id}`, data);
    return response.data;
  },

    delete: async (id) => {
    await api.delete(`/api/quizzes/${id}`);
  },

  getCategories: async () => {
    const response = await api.get('/api/categories');
    return response.data;
  },
};

export default quizService;
