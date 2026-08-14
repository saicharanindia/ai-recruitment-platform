import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AiService } from '../../core/services/ai.service';
import { CandidateDashboardStats } from '../../core/models/models';
import { ScoreBadgeComponent } from '../../shared/components/score-badge.component';

@Component({
  selector: 'app-candidate-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, ScoreBadgeComponent],
  template: `
    <div class="max-w-7xl mx-auto py-8 px-4" *ngIf="stats">
      <h1 class="text-3xl font-bold text-slate-900 mb-6">Candidate Career Hub</h1>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
        <div class="card bg-white border-l-4 border-blue-600">
          <p class="text-xs font-semibold uppercase text-slate-400">Submitted Applications</p>
          <p class="text-3xl font-bold text-slate-900 mt-2">{{ stats.appliedJobsCount }}</p>
        </div>
        <div class="card bg-white border-l-4 border-emerald-500">
          <p class="text-xs font-semibold uppercase text-slate-400">Average AI Match Score</p>
          <p class="text-3xl font-bold text-emerald-600 mt-2">{{ stats.averageMatchScore }}%</p>
        </div>
      </div>

      <div class="card bg-white">
        <h3 class="text-xl font-bold text-slate-900 mb-4">My Application Tracking & Match Insights</h3>
        <div class="overflow-x-auto">
          <table class="w-full text-left text-sm">
            <thead class="bg-slate-50 text-slate-500 border-b">
              <tr>
                <th class="p-3">Job Title</th>
                <th class="p-3">Applied Date</th>
                <th class="p-3">AI Fit Score</th>
                <th class="p-3">Status</th>
              </tr>
            </thead>
            <tbody class="divide-y">
              <tr *ngFor="let app of stats.recentApplications">
                <td class="p-3 font-semibold text-slate-900">{{ app.jobTitle }}</td>
                <td class="p-3 text-slate-500">{{ app.appliedDate | date:'mediumDate' }}</td>
                <td class="p-3"><app-score-badge [score]="app.matchScore"></app-score-badge></td>
                <td class="p-3">
                  <span class="badge" [ngClass]="{
                    'badge-warning': app.status === 'Pending',
                    'badge-success': app.status === 'Shortlisted',
                    'badge-danger': app.status === 'Rejected'
                  }">{{ app.status }}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `
})
export class CandidateDashboardComponent implements OnInit {
  stats: CandidateDashboardStats | null = null;

  constructor(private aiService: AiService) {}

  ngOnInit(): void {
    this.aiService.getCandidateDashboard().subscribe(s => this.stats = s);
  }
}
