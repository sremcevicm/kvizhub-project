/**
 * Category model – represents a quiz category (e.g. Programiranje, Istorija)
 */
export class Category {
  constructor({ id, name, description, quizCount }) {
    this.id = id;
    this.name = name || '';
    this.description = description || '';
    this.quizCount = quizCount || 0;
  }
}

export default Category;
