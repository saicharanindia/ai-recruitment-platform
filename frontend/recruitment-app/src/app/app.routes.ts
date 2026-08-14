import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login.component';
import { RegisterComponent } from './features/auth/register.component';
import { JobListComponent } from './features/jobs/job-list.component';
import { JobDetailComponent } from './features/jobs/job-detail.component';
import { JobCreateComponent } from './features/jobs/job-create.component';
import { RecruiterDashboardComponent } from './features/recruiter-dashboard/recruiter-dashboard.component';
import { CandidateDashboardComponent } from './features/candidate-dashboard/candidate-dashboard.component';
import { authGuard, recruiterGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'jobs', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'jobs', component: JobListComponent },
  { path: 'jobs/create', component: JobCreateComponent, canActivate: [recruiterGuard] },
  { path: 'jobs/:id', component: JobDetailComponent },
  { path: 'recruiter-dashboard', component: RecruiterDashboardComponent, canActivate: [recruiterGuard] },
  { path: 'candidate-dashboard', component: CandidateDashboardComponent, canActivate: [authGuard] },
  { path: '**', redirectTo: 'jobs' }
];
