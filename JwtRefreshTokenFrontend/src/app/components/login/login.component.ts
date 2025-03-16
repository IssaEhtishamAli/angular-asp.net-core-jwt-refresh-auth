import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  template: `
    <h2>Login</h2>
    <form (ngSubmit)="login()">
      <div>
        <label>Email:</label>
        <input type="email" [(ngModel)]="email" name="email" required />
      </div>
      <div>
        <label>Password:</label>
        <input type="password" [(ngModel)]="password" name="password" required />
      </div>
      <button type="submit">Login</button>
    </form>
    <div *ngIf="errorMessage" style="color:red;">{{errorMessage}}</div>
  `
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  login(): void {
    this.authService.login(this.email, this.password).subscribe({
      next: () => {
        // Navigate to products page upon successful login
        this.router.navigate(['/products']);
      },
      error: err => {
        console.error('Login error', err);
        this.errorMessage = 'Login failed. Please check your credentials.';
      }
    });
  }
}
