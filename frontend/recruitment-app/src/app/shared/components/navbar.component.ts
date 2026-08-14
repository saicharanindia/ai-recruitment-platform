import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="bg-slate-900 text-white shadow-md">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between h-16 items-center">
          <div class="flex items-center space-x-3 cursor-pointer" routerLink="/">
            <div class="bg-blue-600 text-white p-2 rounded-lg font-bold text-xl">AI</div>
            <span class="font-bold text-lg tracking-tight">SmartRecruit</span>
          </div>

          <div class="flex items-center space-x-6">
            <a routerLink="/jobs" class="text-gray-300 hover:text-white font-medium">Browse Jobs</a>

            <ng-container *ngIf="auth.isLoggedIn(); else guestLinks">
              <a *ngIf="auth.isRecruiter()" routerLink="/recruiter-dashboard" class="text-gray-300 hover:text-white font-medium">Recruiter Portal</a>
              <a *ngIf="auth.isCandidate()" routerLink="/candidate-dashboard" class="text-gray-300 hover:text-white font-medium">Candidate Portal</a>

              <div class="flex items-center space-x-3 pl-4 border-l border-gray-700">
                <span class="text-sm font-semibold text-blue-400">{{ auth.currentUser()?.fullName }} ({{ auth.currentUser()?.role }})</span>
                <button (click)="logout()" class="bg-gray-800 hover:bg-gray-700 text-gray-300 px-3 py-1.5 rounded-md text-sm font-medium">Logout</button>
              </div>
            </ng-container>

            <ng-template #guestLinks>
              <a routerLink="/login" class="text-gray-300 hover:text-white font-medium">Login</a>
              <a routerLink="/register" class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md font-medium text-sm">Register</a>
            </ng-template>
          </div>
        </div>
      </div>
    </nav>
  `
})
export class NavbarComponent {
  constructor(public auth: AuthService, private router: Router) {}

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
