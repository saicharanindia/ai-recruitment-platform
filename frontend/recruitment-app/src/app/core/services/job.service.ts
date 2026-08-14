import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Job } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class JobService {
  private apiUrl = 'http://localhost:5000/api/jobs';

  constructor(private http: HttpClient) {}

  getJobs(department?: string, skill?: string): Observable<Job[]> {
    let params = new HttpParams();
    if (department) params = params.set('department', department);
    if (skill) params = params.set('skill', skill);
    return this.http.get<Job[]>(this.apiUrl, { params });
  }

  getJobById(id: number): Observable<Job> {
    return this.http.get<Job>(`${this.apiUrl}/${id}`);
  }

  createJob(jobData: any): Observable<Job> {
    return this.http.post<Job>(this.apiUrl, jobData);
  }

  updateJob(id: number, jobData: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, jobData);
  }

  deleteJob(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
