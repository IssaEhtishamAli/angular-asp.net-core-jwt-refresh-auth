import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
    private TOKEN_KEY = 'access_token';
    private baseUrl = 'https://localhost:7248/api/auth'; // Adjust according to your backend URL
  
    constructor(private http: HttpClient) {}
  
    login(email: string, password: string): Observable<{ token: string }> {
      return this.http.post<{ token: string }>(`${this.baseUrl}/login`, { email, password })
        .pipe(
          tap(response => {
            localStorage.setItem(this.TOKEN_KEY, response.token);
          })
        );
    }
  
    getToken(): string | null {
      return localStorage.getItem(this.TOKEN_KEY);
    }
  
    logout(): void {
      localStorage.removeItem(this.TOKEN_KEY);
    }
  }
  