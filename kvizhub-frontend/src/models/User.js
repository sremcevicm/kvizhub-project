/**
 * User model – represents a registered user
 */
export class User {
  constructor({
    id,
    username,
    email,
    profileImageUrl = null,
    role = 'User',
    createdAt
  } = {}) {
    this.id = id;
    this.username = username || '';
    this.email = email || '';
    this.profileImageUrl = profileImageUrl;
    this.role = role;
    this.createdAt = createdAt ? new Date(createdAt) : null;
  }

  get isAdmin() {
    return this.role === 'Admin';
  }

  get initials() {
    if (!this.username) return '?';
    return this.username.charAt(0).toUpperCase();
  }
}

/**
 * Login response data
 */
export class LoginResponse {
  constructor({ accessToken, refreshToken, userId, username, email, role }) {
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
    this.userId = userId;
    this.username = username;
    this.email = email;
    this.role = role;
  }
}

/**
 * Registration data sent to API
 */
export class RegisterData {
  constructor({ username, email, password, profileImageUrl = '' } = {}) {
    this.username = username;
    this.email = email;
    this.password = password;
    this.profileImageUrl = profileImageUrl || null;
  }
}

export default User;
