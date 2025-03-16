import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
    private TOKEN_KEY = 'access_token';
    private baseUrl = 'https://localhost:7144/api/Auth'; // Adjust according to your backend URL
  
  constructor(private http:HttpClient) { }

  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  removeToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }
  login(email: string, password: string): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.baseUrl}/login`, { email, password })
      .pipe(
        tap((response:any) => {
          localStorage.setItem(this.TOKEN_KEY, response.respData);
          console.log("<-----Set Item----->",response.respData)
        })
      );
  }

}
