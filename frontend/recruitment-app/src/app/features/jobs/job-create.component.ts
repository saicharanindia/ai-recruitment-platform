import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { JobService } from '../../core/services/job.service';

@Component({
  selector: 'app-job-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="max-w-2xl mx-auto my-8 card p-8 bg-white rounded-xl shadow-md">
      <h2 class="text-2xl font-bold text-slate-900 mb-6">Post New Job Opening</h2>

      <form [formGroup]="jobForm" (ngSubmit)="onSubmit()" class="space-y-4">
        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Job Title</label>
          <input type="text" formControlName="title" class="w-full px-3 py-2 border rounded-lg text-sm" placeholder="Senior .NET + Angular Engineer">
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-1">Department</label>
            <input type="text" formControlName="department" class="w-full px-3 py-2 border rounded-lg text-sm" placeholder="Engineering">
          </div>
          <div>
            <label class="block text-sm font-medium text-slate-700 mb-1">Location</label>
            <input type="text" formControlName="location" class="w-full px-3 py-2 border rounded-lg text-sm" placeholder="Hyderabad / Remote">
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Job Description</label>
          <textarea formControlName="description" rows="4" class="w-full px-3 py-2 border rounded-lg text-sm" placeholder="Describe role requirements..."></textarea>
        </div>

        <div>
          <label class="block text-sm font-medium text-slate-700 mb-1">Required Skills (Comma separated)</label>
          <input type="text" formControlName="skillsInput" class="w-full px-3 py-2 border rounded-lg text-sm" placeholder="C#, .NET Core, Angular, SQL Server, Docker">
        </div>

        <button type="submit" [disabled]="jobForm.invalid" class="w-full btn-primary mt-4">Publish Job</button>
      </form>
    </div>
  `
})
export class JobCreateComponent {
  jobForm: FormGroup;

  constructor(private fb: FormBuilder, private jobService: JobService, private router: Router) {
    this.jobForm = this.fb.group({
      title: ['', Validators.required],
      department: ['', Validators.required],
      location: ['', Validators.required],
      description: ['', Validators.required],
      skillsInput: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.jobForm.invalid) return;
    const skills = this.jobForm.value.skillsInput.split(',').map((s: string) => s.trim()).filter((s: string) => s);

    const payload = {
      ...this.jobForm.value,
      employmentType: 'Full-time',
      requiredSkills: skills
    };

    this.jobService.createJob(payload).subscribe(() => {
      this.router.navigate(['/jobs']);
    });
  }
}
