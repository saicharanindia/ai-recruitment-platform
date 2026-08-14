import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AiService } from '../../core/services/ai.service';
import { ApplicationService } from '../../core/services/application.service';
import { Application, InterviewQuestion, RecruiterDashboardStats } from '../../core/models/models';
import { ScoreBadgeComponent } from '../../shared/components/score-badge.component';

@Component({
  selector: 'app-recruiter-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, ScoreBadgeComponent],
  template: `
    <div class="max-w-7xl mx-auto py-8 px-4" *ngIf="stats">
      <h1 class="text-3xl font-bold text-slate-900 mb-6">Recruiter Management Portal</h1>

      <!-- Stats Grid -->
      <div class="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
        <div class="card bg-white border-l-4 border-blue-600">
          <p class="text-xs font-semibold uppercase text-slate-400">Total Active Jobs</p>
          <p class="text-3xl font-bold text-slate-900 mt-2">{{ stats.totalJobs }}</p>
        </div>
        <div class="card bg-white border-l-4 border-amber-500">
          <p class="text-xs font-semibold uppercase text-slate-400">Pending Review</p>
          <p class="text-3xl font-bold text-slate-900 mt-2">{{ stats.pendingApplications }}</p>
        </div>
        <div class="card bg-white border-l-4 border-emerald-500">
          <p class="text-xs font-semibold uppercase text-slate-400">Shortlisted</p>
          <p class="text-3xl font-bold text-slate-900 mt-2">{{ stats.shortlistedCandidates }}</p>
        </div>
        <div class="card bg-white border-l-4 border-rose-500">
          <p class="text-xs font-semibold uppercase text-slate-400">Rejected</p>
          <p class="text-3xl font-bold text-slate-900 mt-2">{{ stats.rejectedCandidates }}</p>
        </div>
      </div>

      <!-- Applicant Pool Table -->
      <div class="card bg-white mb-8">
        <h3 class="text-xl font-bold text-slate-900 mb-4">Applicant Screening Pipeline</h3>
        <div class="overflow-x-auto">
          <table class="w-full text-left text-sm">
            <thead class="bg-slate-50 text-slate-500 border-b">
              <tr>
                <th class="p-3">Candidate Name</th>
                <th class="p-3">Position</th>
                <th class="p-3">AI Fit Score</th>
                <th class="p-3">Status</th>
                <th class="p-3">Actions</th>
              </tr>
            </thead>
            <tbody class="divide-y">
              <tr *ngFor="let app of applications">
                <td class="p-3 font-semibold text-slate-900">{{ app.candidateName }}</td>
                <td class="p-3 text-slate-600">{{ app.jobTitle }}</td>
                <td class="p-3"><app-score-badge [score]="app.matchScore"></app-score-badge></td>
                <td class="p-3">
                  <span class="badge" [ngClass]="{
                    'badge-warning': app.status === 'Pending',
                    'badge-success': app.status === 'Shortlisted',
                    'badge-danger': app.status === 'Rejected'
                  }">{{ app.status }}</span>
                </td>
                <td class="p-3 space-x-2">
                  <button (click)="updateStatus(app, 'Shortlisted')" class="text-xs bg-emerald-50 text-emerald-700 px-2.5 py-1 rounded font-semibold border border-emerald-200 hover:bg-emerald-100">Approve</button>
                  <button (click)="updateStatus(app, 'Rejected')" class="text-xs bg-rose-50 text-rose-700 px-2.5 py-1 rounded font-semibold border border-rose-200 hover:bg-rose-100">Reject</button>
                  <button (click)="generateQuestions(app)" class="text-xs bg-blue-50 text-blue-700 px-2.5 py-1 rounded font-semibold border border-blue-200 hover:bg-blue-100">Generate AI Interview Qs</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Generated Questions Modal View -->
      <div *ngIf="selectedQuestions.length > 0" class="card bg-slate-900 text-white p-6 rounded-xl">
        <h3 class="text-lg font-bold mb-3 text-blue-400">🤖 AI-Generated Interview Questions</h3>
        <ul class="space-y-3">
          <li *ngFor="let q of selectedQuestions" class="bg-slate-800 p-3 rounded-lg">
            <span class="text-xs font-semibold px-2 py-0.5 bg-blue-600 text-white rounded mr-2">{{ q.category }}</span>
            <span>{{ q.questionText }}</span>
          </li>
        </ul>
      </div>
    </div>
  `
})
export class RecruiterDashboardComponent implements OnInit {
  stats: RecruiterDashboardStats | null = null;
  applications: Application[] = [];
  selectedQuestions: InterviewQuestion[] = [];

  constructor(private aiService: AiService, private appService: ApplicationService) {}

  ngOnInit(): void {
    this.aiService.getRecruiterDashboard().subscribe(s => this.stats = s);
    this.appService.getApplications().subscribe(apps => this.applications = apps);
  }

  updateStatus(app: Application, newStatus: 'Shortlisted' | 'Rejected'): void {
    this.appService.updateStatus(app.applicationId, newStatus).subscribe(() => {
      app.status = newStatus;
    });
  }

  generateQuestions(app: Application): void {
    this.aiService.generateQuestions(app.applicationId).subscribe(res => {
      this.selectedQuestions = res.questions;
    });
  }
}
