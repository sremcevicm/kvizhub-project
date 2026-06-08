import api from './api';
import { Quiz } from '../models/Quiz';
import { Question } from '../models/Question';
import { Category } from '../models/Category';

const mapQuiz = (item) => new Quiz(item);
const mapQuizList = (data) => {
  const items = data?.value || data || [];
  return Array.isArray(items) ? items.map(mapQuiz) : [];
};

const mapQuestions = (data) => {
  if (!data) return [];
  return Array.isArray(data) ? data.map(q => new Question(q)) : [];
};

const mapCategories = (data) => {
  const items = data?.value || data || [];
  return Array.isArray(items) ? items.map(c => new Category(c)) : [];
};

const quizService = {
  getAll: async () => {
    const response = await api.get('/api/quizzes');
    return mapQuizList(response.data);
  },

  getFiltered: async (categoryId, difficulty, search) => {
    const params = new URLSearchParams();
    if (categoryId) params.append('categoryId', categoryId);
    if (difficulty) params.append('difficulty', difficulty);
    if (search) params.append('search', search);
    const response = await api.get(`/api/quizzes/filter?${params.toString()}`);
    return mapQuizList(response.data);
  },

  getById: async (id) => {
    const response = await api.get(`/api/quizzes/${id}`);
    return new Quiz(response.data);
  },

  getQuestionsForPlayer: async (id) => {
    const response = await api.get(`/api/quizzes/${id}/play`);
    return mapQuestions(response.data);
  },

  getQuestionsWithAnswers: async (id) => {
    const response = await api.get(`/api/quizzes/${id}/questions`);
    return mapQuestions(response.data);
  },

  create: async (data) => {
    const response = await api.post('/api/quizzes', data);
    return new Quiz(response.data);
  },

  update: async (id, data) => {
    const response = await api.put(`/api/quizzes/${id}`, data);
    return new Quiz(response.data);
  },

  delete: async (id) => {
    await api.delete(`/api/quizzes/${id}`);
  },

  getCategories: async () => {
    const response = await api.get('/api/categories');
    return mapCategories(response.data);
  },
};

export default quizService;
