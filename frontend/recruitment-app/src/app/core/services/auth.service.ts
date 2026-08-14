import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { AuthResponse, UserProfile } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api/auth';
  public currentUser = signal<UserProfile | null>(null);

  constructor(private http: HttpClient) {
    const saved = localStorage.getItem('user_profile');
    if (saved) {
      try {
        this.currentUser.set(JSON.parse(saved));
      } catch {}
    }
  }

  register(data: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, data).pipe(
      tap(res => this.handleAuthSuccess(res))
    );
  }

  login(credentials: { email: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap(res => this.handleAuthSuccess(res))
    );
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_profile');
    this.currentUser.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  isRecruiter(): boolean {
    return this.currentUser()?.role === 'Recruiter';
  }

  isCandidate(): boolean {
    return this.currentUser()?.role === 'Candidate';
  }

  private handleAuthSuccess(res: AuthResponse): void {
    localStorage.setItem('jwt_token', res.token);
    const profile: UserProfile = {
      userId: res.userId,
      fullName: res.fullName,
      email: res.email,
      role: res.role as 'Candidate' | 'Recruiter'
    };
    localStorage.setItem('user_profile', JSON.stringify(profile));
    this.currentUser.set(profile);
  }
}
