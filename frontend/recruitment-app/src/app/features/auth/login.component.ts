import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  template: `
    <div class="max-w-md mx-auto my-12 card p-8 bg-white rounded-xl shadow-md">
      <h2 class="text-2xl font-bold text-slate-900 mb-2">Welcome Back</h2>
      <p class="text-slate-500 mb-6 text-sm">Sign in to your recruitment account</p>

      <div *ngIf="errorMessage" class="bg-red-50 text-red-700 p-3 rounded-lg text-sm mb-4">
        {{ errorMessage }}
      </div>

      <form [formGroup]="loginForm" (ngSubmit)="onSubmit()" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Email Address</label>
          <input type="email" formControlName="email" class="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 outline-none text-sm" placeholder="you@example.com">
        </div>

        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Password</label>
          <input type="password" formControlName="password" class="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 outline-none text-sm" placeholder="••••••••">
        </div>

        <button type="submit" [disabled]="loginForm.invalid || loading" class="w-full btn-primary mt-2">
          {{ loading ? 'Signing in...' : 'Sign In' }}
        </button>
      </form>

      <p class="text-xs text-center text-slate-500 mt-6">
        Don't have an account? <a routerLink="/register" class="text-blue-600 font-semibold">Register here</a>
      </p>
    </div>
  `
})
export class LoginComponent {
  loginForm: FormGroup;
  loading = false;
  errorMessage = '';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) return;
    this.loading = true;
    this.errorMessage = '';

    this.authService.login(this.loginForm.value).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.role === 'Recruiter') this.router.navigate(['/recruiter-dashboard']);
        else this.router.navigate(['/candidate-dashboard']);
      },
      error: (err) => {
        this.loading = false;
        this.errorMessage = err.error?.message || 'Invalid email or password.';
      }
    });
  }
}
