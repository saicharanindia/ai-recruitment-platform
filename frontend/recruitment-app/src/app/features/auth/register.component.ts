import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  template: `
    <div class="max-w-lg mx-auto my-12 card p-8 bg-white rounded-xl shadow-md">
      <h2 class="text-2xl font-bold text-slate-900 mb-2">Create Account</h2>
      <p class="text-slate-500 mb-6 text-sm">Join the AI-driven recruitment platform</p>

      <div *ngIf="errorMessage" class="bg-red-50 text-red-700 p-3 rounded-lg text-sm mb-4">
        {{ errorMessage }}
      </div>

      <form [formGroup]="regForm" (ngSubmit)="onSubmit()" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Full Name</label>
          <input type="text" formControlName="fullName" class="w-full px-3 py-2 border rounded-lg text-sm" placeholder="John Doe">
        </div>

        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Email Address</label>
          <input type="email" formControlName="email" class="w-full px-3 py-2 border rounded-lg text-sm" placeholder="name@example.com">
        </div>

        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Password</label>
          <input type="password" formControlName="password" class="w-full px-3 py-2 border rounded-lg text-sm" placeholder="••••••••">
        </div>

        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Account Role</label>
          <select formControlName="role" class="w-full px-3 py-2 border rounded-lg text-sm">
            <option value="Candidate">Candidate (Job Seeker)</option>
            <option value="Recruiter">Recruiter (Hiring Manager)</option>
          </select>
        </div>

        <button type="submit" [disabled]="regForm.invalid || loading" class="w-full btn-primary mt-2">
          {{ loading ? 'Creating...' : 'Register' }}
        </button>
      </form>
    </div>
  `
})
export class RegisterComponent {
  regForm: FormGroup;
  loading = false;
  errorMessage = '';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.regForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['Candidate', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.regForm.invalid) return;
    this.loading = true;

    this.authService.register(this.regForm.value).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.role === 'Recruiter') this.router.navigate(['/recruiter-dashboard']);
        else this.router.navigate(['/candidate-dashboard']);
      },
      error: (err) => {
        this.loading = false;
        this.errorMessage = err.error?.message || 'Registration failed.';
      }
    });
  }
}
