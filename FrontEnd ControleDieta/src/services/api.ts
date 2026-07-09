// Helper service for managing authentication and API requests

export interface UserTokenPayload {
  userId: number;
  userName: string;
  email: string;
  roles: string[];
}

export function decodeJwt(token: string): UserTokenPayload | null {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      window
        .atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    
    const parsed = JSON.parse(jsonPayload);
    
    // .NET claims mappings
    const userIdClaim = parsed['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || parsed.nameid || parsed.sub;
    const userNameClaim = parsed['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || parsed.unique_name || parsed.name;
    const emailClaim = parsed['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || parsed.email;
    
    // Roles can be a string or an array of strings
    const roleClaim = parsed['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role'] || parsed.role || [];
    const roles = Array.isArray(roleClaim) ? roleClaim : [roleClaim];
    
    return {
      userId: userIdClaim ? parseInt(userIdClaim, 10) : 0,
      userName: userNameClaim || '',
      email: emailClaim || '',
      roles
    };
  } catch (error) {
    console.error('Failed to decode JWT token:', error);
    return null;
  }
}

export const authService = {
  getToken(): string | null {
    return localStorage.getItem('diet_auth_token');
  },
  
  setToken(token: string) {
    localStorage.setItem('diet_auth_token', token);
  },
  
  logout() {
    localStorage.removeItem('diet_auth_token');
  },
  
  getUser(): UserTokenPayload | null {
    const token = this.getToken();
    if (!token) return null;
    return decodeJwt(token);
  },
  
  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    
    // Simple expiration check
    try {
      const parts = token.split('.');
      if (parts.length < 2) return false;
      const payload = JSON.parse(window.atob(parts[1]));
      if (payload.exp && Date.now() >= payload.exp * 1000) {
        this.logout();
        return false;
      }
      return true;
    } catch {
      return false;
    }
  }
};

export async function apiFetch<T>(
  url: string,
  options: RequestInit = {}
): Promise<T> {
  const token = authService.getToken();
  const headers = new Headers(options.headers || {});
  
  if (token) {
    headers.set('Authorization', `Bearer ${token}`);
  }
  
  if (!(options.body instanceof FormData) && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json');
  }
  
  const response = await fetch(url, {
    ...options,
    headers
  });
  
  if (!response.ok) {
    const errorText = await response.text();
    let errorMessage = `Request failed: ${response.statusText}`;
    try {
      const parsedError = JSON.parse(errorText);
      errorMessage = parsedError.message || parsedError.title || errorText || errorMessage;
    } catch {
      if (errorText) errorMessage = errorText;
    }
    throw new Error(errorMessage);
  }
  
  // Handlers for empty or JSON responses
  const contentType = response.headers.get('content-type');
  if (contentType && contentType.includes('application/json')) {
    return response.json() as Promise<T>;
  }
  
  return {} as Promise<T>;
}
