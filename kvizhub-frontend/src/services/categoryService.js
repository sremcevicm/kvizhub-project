import api from './api';
import { Category } from '../models/Category';

const mapCategory = (item) => new Category(item);
const mapCategoryList = (data) => {
  const items = data?.value || data || [];
  return Array.isArray(items) ? items.map(mapCategory) : [];
};

const categoryService = {
  getAll: async () => {
    const response = await api.get('/api/categories');
    return mapCategoryList(response.data);
  },

  getById: async (id) => {
    const response = await api.get(`/api/categories/${id}`);
    return mapCategory(response.data);
  },

  create: async (data) => {
    const response = await api.post('/api/categories', data);
    return mapCategory(response.data);
  },

  update: async (id, data) => {
    const response = await api.put(`/api/categories/${id}`, data);
    return mapCategory(response.data);
  },

  delete: async (id) => {
    await api.delete(`/api/categories/${id}`);
  },
};

export default categoryService;
