using Recruitment.Application.DTOs;

namespace Recruitment.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<UserProfileDto?> GetUserProfileAsync(int userId);
}

public interface IJobService
{
    Task<IEnumerable<JobResponseDto>> GetAllJobsAsync(string? department, string? skill);
    Task<JobResponseDto?> GetJobByIdAsync(int jobId);
    Task<JobResponseDto> CreateJobAsync(int recruiterUserId, JobCreateDto dto);
    Task<bool> UpdateJobAsync(int jobId, int recruiterUserId, JobCreateDto dto);
    Task<bool> DeleteJobAsync(int jobId, int recruiterUserId);
}

public interface IApplicationService
{
    Task<ApplicationResponseDto> ApplyToJobAsync(int candidateUserId, ApplyJobRequestDto dto);
    Task<IEnumerable<ApplicationResponseDto>> GetApplicationsForUserAsync(int userId, string role);
    Task<bool> UpdateApplicationStatusAsync(int applicationId, int recruiterUserId, UpdateStatusRequestDto dto);
}

public interface IAiService
{
    Task<ResumeParsedDataDto> AnalyzeResumeAsync(string resumeText);
    Task<AiMatchResultDto> MatchResumeToJobAsync(MatchResumeRequestDto request);
    Task<InterviewQuestionsResponseDto> GenerateInterviewQuestionsAsync(int applicationId);
    Task<RecruiterDashboardDto> GetRecruiterDashboardAsync(int recruiterUserId);
    Task<CandidateDashboardDto> GetCandidateDashboardAsync(int candidateUserId);
}
