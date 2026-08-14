import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { JobService } from '../../core/services/job.service';
import { Job } from '../../core/models/models';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-job-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="max-w-7xl mx-auto py-8 px-4">
      <div class="flex justify-between items-center mb-8">
        <div>
          <h1 class="text-3xl font-bold text-slate-900">Explore Open Positions</h1>
          <p class="text-slate-500 mt-1">Discover opportunities matched with AI screening</p>
        </div>
        <a *ngIf="auth.isRecruiter()" routerLink="/jobs/create" class="btn-primary flex items-center space-x-2">
          <span>+ Post New Job</span>
        </a>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div *ngFor="let job of jobs" class="card bg-white hover:shadow-lg transition border border-slate-200 flex flex-col justify-between">
          <div>
            <div class="flex justify-between items-start mb-2">
              <span class="text-xs font-semibold uppercase px-2.5 py-1 bg-blue-50 text-blue-700 rounded-md">{{ job.department }}</span>
              <span class="text-xs text-slate-400">{{ job.employmentType }}</span>
            </div>
            <h3 class="text-xl font-bold text-slate-900 mb-2">{{ job.title }}</h3>
            <p class="text-sm text-slate-600 mb-4 line-clamp-3">{{ job.description }}</p>

            <div class="flex flex-wrap gap-1.5 mb-4">
              <span *ngFor="let s of job.requiredSkills" class="text-xs bg-slate-100 text-slate-700 px-2 py-0.5 rounded">
                {{ s }}
              </span>
            </div>
          </div>

          <div class="pt-4 border-t border-slate-100 flex justify-between items-center">
            <span class="text-sm font-bold text-slate-800">
              {{ job.salaryMin ? '$' + (job.salaryMin/1000) + 'k' : '' }} {{ job.salaryMax ? '- $' + (job.salaryMax/1000) + 'k' : '' }}
            </span>
            <a [routerLink]="['/jobs', job.jobId]" class="text-sm font-semibold text-blue-600 hover:text-blue-800">View Details &rarr;</a>
          </div>
        </div>
      </div>
    </div>
  `
})
export class JobListComponent implements OnInit {
  jobs: Job[] = [];

  constructor(public jobService: JobService, public auth: AuthService) {}

  ngOnInit(): void {
    this.jobService.getJobs().subscribe({
      next: (data) => this.jobs = data,
      error: (err) => console.error(err)
    });
  }
}
