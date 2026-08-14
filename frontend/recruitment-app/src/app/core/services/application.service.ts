import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Application } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {
  private apiUrl = 'http://localhost:5000/api/applications';

  constructor(private http: HttpClient) {}

  apply(jobId: number): Observable<Application> {
    return this.http.post<Application>(this.apiUrl, { jobId });
  }

  getApplications(): Observable<Application[]> {
    return this.http.get<Application[]>(this.apiUrl);
  }

  updateStatus(id: number, status: 'Pending' | 'Shortlisted' | 'Rejected'): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/status`, { status });
  }
}
