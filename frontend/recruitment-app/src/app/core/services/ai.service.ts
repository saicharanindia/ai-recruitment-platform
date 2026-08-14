import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AiMatchResult, CandidateDashboardStats, InterviewQuestion, RecruiterDashboardStats } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class AiService {
  private apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) {}

  matchResume(resumeText: string, jobDescription: string, requiredSkills: string[]): Observable<AiMatchResult> {
    return this.http.post<AiMatchResult>(`${this.apiUrl}/ai/match-resume`, { resumeText, jobDescription, requiredSkills });
  }

  generateQuestions(applicationId: number): Observable<{ questions: InterviewQuestion[] }> {
    return this.http.post<{ questions: InterviewQuestion[] }>(`${this.apiUrl}/ai/generate-questions`, { applicationId });
  }

  getRecruiterDashboard(): Observable<RecruiterDashboardStats> {
    return this.http.get<RecruiterDashboardStats>(`${this.apiUrl}/dashboard/recruiter`);
  }

  getCandidateDashboard(): Observable<CandidateDashboardStats> {
    return this.http.get<CandidateDashboardStats>(`${this.apiUrl}/dashboard/candidate`);
  }
}
