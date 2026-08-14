using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs;

// Auth DTOs
public record RegisterRequestDto(string FullName, string Email, string Password, UserRole Role, string? Department, string? Title, string? Phone);
public record LoginRequestDto(string Email, string Password);
public record AuthResponseDto(int UserId, string FullName, string Email, string Role, string Token, int ExpiresIn);
public record UserProfileDto(int UserId, string FullName, string Email, string Role, string? Phone, string? Department, string? Title);

// Job DTOs
public record JobCreateDto(string Title, string Description, string Department, string Location, string EmploymentType, double? SalaryMin, double? SalaryMax, List<string> RequiredSkills);
public record JobResponseDto(int JobId, int RecruiterId, string Title, string Description, string Department, string Location, string EmploymentType, double? SalaryMin, double? SalaryMax, DateTime CreatedDate, bool IsActive, List<string> RequiredSkills);

// Application DTOs
public record ApplyJobRequestDto(int JobId);
public record ApplicationResponseDto(int ApplicationId, int JobId, string JobTitle, int CandidateId, string CandidateName, DateTime AppliedDate, ApplicationStatus Status, int? MatchScore, List<string>? MatchedSkills, List<string>? MissingSkills);
public record UpdateStatusRequestDto(ApplicationStatus Status);

// Resume DTOs
public record ResumeUploadResponseDto(int ResumeId, string FilePath, DateTime UploadedAt);
public record ResumeParsedDataDto(string CandidateName, string Email, List<string> Skills, List<string> ExperienceSummary);

// AI DTOs
public record AnalyzeResumeRequestDto(string ResumeText);
public record AnalyzeJobRequestDto(string JobDescription);
public record MatchResumeRequestDto(string ResumeText, string JobDescription, List<string> RequiredSkills);
public record AiMatchResultDto(int MatchScore, List<string> MatchedSkills, List<string> MissingSkills, string Recommendation);
public record GenerateQuestionsRequestDto(int ApplicationId);
public record QuestionDto(string QuestionText, string Category);
public record InterviewQuestionsResponseDto(List<QuestionDto> Questions);

// Dashboard DTOs
public record RecruiterDashboardDto(int TotalJobs, int PendingApplications, int ShortlistedCandidates, int RejectedCandidates, List<JobResponseDto> ActiveJobs);
public record CandidateDashboardDto(int AppliedJobsCount, double AverageMatchScore, List<ApplicationResponseDto> RecentApplications);
