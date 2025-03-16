import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, filter, switchMap, take, tap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string|null> = new BehaviorSubject<string|null>(null);

  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Attach the token to the request headers if available.
    const token = this.authService.getToken();
    let authReq = req;
    if (token) {
      authReq = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
    }

    return next.handle(authReq).pipe(
      tap(event => {
        // If the server sends a new token in the response headers, update it.
        if (event instanceof HttpResponse) {
          const newToken = event.headers.get('Authorization');
          if (newToken) {
            // The server might return the token as "Bearer <token>" so extract if necessary.
            const tokenParts = newToken.split(' ');
            if (tokenParts.length > 1) {
              this.authService.setToken(tokenParts[1]);
            } else {
              this.authService.setToken(newToken);
            }
          }
        }
      }),
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          // Token is expired or unauthorized. Here you can call a refresh token API if available.
          // For now, we'll assume no refresh token flow and log the user out.
          this.authService.removeToken();
          // Optionally, redirect to the login page.
          // For example: this.router.navigate(['/login']);
        }
        return throwError(error);
      })
    );
  }
}
