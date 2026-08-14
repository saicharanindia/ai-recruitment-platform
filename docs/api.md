# RESTful API Specification

Base URL: `http://localhost:5000/api`

## Authentication & Headers
All protected endpoints require HTTP Bearer Token authentication:
`Authorization: Bearer <JWT_TOKEN>`

## Endpoints Table

| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| **POST** | `/api/auth/register` | Public | Register new Candidate or Recruiter account |
| **POST** | `/api/auth/login` | Public | Authenticate credentials & return JWT |
| **GET** | `/api/users/me` | Authenticated | Get current user's profile |
| **GET** | `/api/jobs` | Public/Auth | List jobs (with search/filter query params) |
| **POST** | `/api/jobs` | Recruiter | Create a new job posting |
| **GET** | `/api/jobs/{id}` | Public/Auth | Get detailed job posting information |
| **PUT** | `/api/jobs/{id}` | Recruiter | Update job details |
| **DELETE**| `/api/jobs/{id}` | Recruiter | Soft-delete/deactivate job posting |
| **POST** | `/api/resumes/upload` | Candidate | Upload candidate resume document (PDF/DOCX) |
| **GET** | `/api/resumes/{id}` | Auth | Retrieve parsed resume details |
| **POST** | `/api/applications` | Candidate | Submit job application |
| **GET** | `/api/applications` | Auth | Get applications (Candidate's own or Recruiter's jobs) |
| **PUT** | `/api/applications/{id}/status` | Recruiter | Update status (Pending, Shortlisted, Rejected) |
| **POST** | `/api/ai/analyze-resume` | Auth | Parse raw resume text into structured JSON |
| **POST** | `/api/ai/match-resume` | Auth | Compute match score & skill gaps |
| **POST** | `/api/interviews/generate` | Recruiter | Generate tailored interview questions |
| **GET** | `/api/dashboard/recruiter` | Recruiter | Aggregate metrics & pipeline stats |
| **GET** | `/api/dashboard/candidate` | Candidate | Candidate dashboard summary |

## Example API Payloads

### POST /api/auth/login Response
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 1,
  "fullName": "Jane Doe",
  "email": "recruiter@cognizant.com",
  "role": "Recruiter",
  "expiresIn": 3600
}
```

### POST /api/ai/match-resume Response
```json
{
  "matchScore": 88,
  "matchedSkills": ["C#", ".NET Core", "Angular", "TypeScript"],
  "missingSkills": ["Entity Framework Core"],
  "recommendation": "Shortlist"
}
```
