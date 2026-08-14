export interface UserProfile {
  userId: number;
  fullName: string;
  email: string;
  role: 'Candidate' | 'Recruiter';
  phone?: string;
  department?: string;
  title?: string;
}

export interface AuthResponse {
  userId: number;
  fullName: string;
  email: string;
  role: string;
  token: string;
  expiresIn: number;
}

export interface Job {
  jobId: number;
  recruiterId: number;
  title: string;
  description: string;
  department: string;
  location: string;
  employmentType: string;
  salaryMin?: number;
  salaryMax?: number;
  createdDate: string;
  isActive: boolean;
  requiredSkills: string[];
}

export interface Application {
  applicationId: number;
  jobId: number;
  jobTitle: string;
  candidateId: number;
  candidateName: string;
  appliedDate: string;
  status: 'Pending' | 'Shortlisted' | 'Rejected';
  matchScore?: number;
  matchedSkills?: string[];
  missingSkills?: string[];
}

export interface AiMatchResult {
  matchScore: number;
  matchedSkills: string[];
  missingSkills: string[];
  recommendation: string;
}

export interface InterviewQuestion {
  questionText: string;
  category: string;
}

export interface RecruiterDashboardStats {
  totalJobs: number;
  pendingApplications: number;
  shortlistedCandidates: number;
  rejectedCandidates: number;
  activeJobs: Job[];
}

export interface CandidateDashboardStats {
  appliedJobsCount: number;
  averageMatchScore: number;
  recentApplications: Application[];
}
