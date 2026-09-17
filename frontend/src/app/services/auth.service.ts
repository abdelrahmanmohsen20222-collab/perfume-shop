import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, tap, throwError, TimeoutError, timeout } from 'rxjs';

interface AuthResponse {
  token?: string;
  accessToken?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7096/api/Auth';
  private readonly requestTimeoutMs = 3000;

  constructor(private http: HttpClient) {}

  login(username: string, password: string) {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/login`,
      {
        username,
        password
      }
    ).pipe(
      timeout(this.requestTimeoutMs),
      catchError((error) => {
        if (error instanceof TimeoutError) {
          return throwError(() => new Error('The server is taking too long to respond. Please try again.'));
        }

        if (error instanceof HttpErrorResponse && [400, 401, 403].includes(error.status)) {
          return throwError(() => new Error('Invalid username or password.'));
        }

        if (error instanceof HttpErrorResponse && error.status === 0) {
          return throwError(() => new Error('Cannot reach the server. Check that the backend is running.'));
        }

        return throwError(() => new Error('Sign-in failed. Please try again.'));
      }),
      tap(response => {
        const token = response.token ?? response.accessToken;

        if (!token) {
          throw new Error('The server did not return a sign-in token.');
        }

        localStorage.setItem('token', token);
      })
    );
  }

  register(username: string, password: string) {
    return this.http.post(
      `${this.apiUrl}/register`,
      {
        username,
        password
      },
      { responseType: 'text' }
    ).pipe(
      timeout(this.requestTimeoutMs),
      catchError((error) => {
        if (error instanceof TimeoutError) {
          return throwError(() => new Error('The server is taking too long to respond. Please try again.'));
        }

        if (error instanceof HttpErrorResponse && error.status === 0) {
          return throwError(() => new Error('Cannot reach the server. Check that the backend is running.'));
        }

        if (error instanceof HttpErrorResponse && error.status === 409) {
          return throwError(() => new Error('That username is already registered.'));
        }

        if (error instanceof HttpErrorResponse && error.status === 400) {
          return throwError(() => new Error('Please choose a valid username and password.'));
        }

        return throwError(() => new Error('Unable to create the account right now.'));
      })
    );
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  isAdmin(): boolean {
    const token = this.getToken();

    if (!token) {
      return false;
    }

    try {
      const payload = JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')));
      const role = payload.role ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return Array.isArray(role) ? role.includes('Admin') : role === 'Admin';
    } catch {
      return false;
    }
  }
}
