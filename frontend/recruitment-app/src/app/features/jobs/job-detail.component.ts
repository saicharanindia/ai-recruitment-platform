import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { JobService } from '../../core/services/job.service';
import { ApplicationService } from '../../core/services/application.service';
import { Job } from '../../core/models/models';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-job-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="max-w-4xl mx-auto py-8 px-4" *ngIf="job">
      <div class="card bg-white p-8 mb-6">
        <div class="flex justify-between items-start mb-4">
          <div>
            <span class="text-xs font-semibold uppercase px-2.5 py-1 bg-blue-50 text-blue-700 rounded-md">{{ job.department }}</span>
            <h1 class="text-3xl font-bold text-slate-900 mt-2">{{ job.title }}</h1>
            <p class="text-slate-500 text-sm mt-1">📍 {{ job.location }} &bull; {{ job.employmentType }}</p>
          </div>
          <button *ngIf="auth.isCandidate()" (click)="applyJob()" [disabled]="applied" class="btn-primary text-base px-6 py-2.5">
            {{ applied ? 'Applied ✓' : 'Apply Now' }}
          </button>
        </div>

        <hr class="my-6 border-slate-100">

        <h3 class="text-lg font-bold text-slate-800 mb-2">Job Description</h3>
        <p class="text-slate-600 leading-relaxed whitespace-pre-line mb-6">{{ job.description }}</p>

        <h3 class="text-lg font-bold text-slate-800 mb-2">Required Core Skills</h3>
        <div class="flex flex-wrap gap-2 mb-6">
          <span *ngFor="let s of job.requiredSkills" class="bg-blue-50 text-blue-800 font-medium px-3 py-1 rounded-full text-sm">
            {{ s }}
          </span>
        </div>
      </div>
    </div>
  `
})
export class JobDetailComponent implements OnInit {
  job: Job | null = null;
  applied = false;

  constructor(
    private route: ActivatedRoute,
    private jobService: JobService,
    private appService: ApplicationService,
    public auth: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.jobService.getJobById(id).subscribe(j => this.job = j);
    }
  }

  applyJob(): void {
    if (!this.job) return;
    this.appService.apply(this.job.jobId).subscribe({
      next: () => {
        this.applied = true;
        alert('Application submitted successfully! AI match scoring in progress.');
      },
      error: (err) => alert(err.error?.message || 'Error submitting application.')
    });
  }
}
