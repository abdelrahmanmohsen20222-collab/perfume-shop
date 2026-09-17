import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, throwError, timeout } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private apiUrl = 'https://localhost:7096/api';

  constructor(private http: HttpClient) {}

  login(username: string, password: string) {
    return this.http.post<{ token: string }>(
      `${this.apiUrl}/Auth/login`,
      {
        username,
        password
      }
    );
  }

  register(username: string, password: string) {
    return this.http.post(
      `${this.apiUrl}/Auth/register`,
      {
        username,
        password
      }
    );
  }

  getPerfumes() {
    return this.http.get<any[]>(
      `${this.apiUrl}/Perfumes`
    );
  }

  findScent(preferences: { mood: string; notes: string; occasion: string; strength: string }) {
    return this.http.post<{ recommendation: string }>(
      `${this.apiUrl}/ScentFinder/recommend`,
      preferences
    ).pipe(
      timeout(30000),
      catchError(() => throwError(() => new Error('The Scent Finder is unavailable right now. Please try again.')))
    );
  }
}
