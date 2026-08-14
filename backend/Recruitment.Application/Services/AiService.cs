using System.Text.Json;
using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Interfaces;

namespace Recruitment.Application.Services;

public class AiService : IAiService
{
    private readonly IAiServiceClient _client;
    private readonly IUnitOfWork _unitOfWork;

    public AiService(IAiServiceClient client, IUnitOfWork unitOfWork)
    {
        _client = client;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResumeParsedDataDto> AnalyzeResumeAsync(string resumeText)
    {
        return await _client.ParseResumeAsync(resumeText);
    }

    public async Task<AiMatchResultDto> MatchResumeToJobAsync(MatchResumeRequestDto request)
    {
        return await _client.ComputeMatchAsync(request);
    }

    public async Task<InterviewQuestionsResponseDto> GenerateInterviewQuestionsAsync(int applicationId)
    {
        var app = await _unitOfWork.Applications.GetByIdAsync(applicationId);
        if (app == null) throw new KeyNotFoundException("Application not found.");

        var job = await _unitOfWork.Jobs.GetByIdAsync(app.JobId);
        var aiMatches = await _unitOfWork.AiMatches.FindAsync(m => m.ApplicationId == applicationId);
        var aiMatch = aiMatches.FirstOrDefault();

        List<string> missingSkills = aiMatch?.MissingSkills != null
            ? JsonSerializer.Deserialize<List<string>>(aiMatch.MissingSkills) ?? new List<string>()
            : new List<string>();

        var questionDtos = await _client.GenerateQuestionsAsync(job?.Title ?? "Software Engineer", missingSkills);

        foreach (var q in questionDtos)
        {
            var entity = new InterviewQuestion
            {
                ApplicationId = applicationId,
                QuestionText = q.QuestionText,
                Category = q.Category
            };
            await _unitOfWork.InterviewQuestions.AddAsync(entity);
        }
        await _unitOfWork.CompleteAsync();

        return new InterviewQuestionsResponseDto(questionDtos);
    }

    public async Task<RecruiterDashboardDto> GetRecruiterDashboardAsync(int recruiterUserId)
    {
        var recruiters = await _unitOfWork.Recruiters.FindAsync(r => r.UserId == recruiterUserId);
        var recruiter = recruiters.FirstOrDefault();
        if (recruiter == null) throw new InvalidOperationException("Recruiter profile not found.");

        var jobs = await _unitOfWork.Jobs.FindAsync(j => j.RecruiterId == recruiter.RecruiterId);
        var jobIds = jobs.Select(j => j.JobId).ToHashSet();

        var apps = (await _unitOfWork.Applications.GetAllAsync()).Where(a => jobIds.Contains(a.JobId)).ToList();

        var activeJobDtos = jobs.Where(j => j.IsActive).Select(j => new JobResponseDto(
            j.JobId, j.RecruiterId, j.Title, j.Description, j.Department, j.Location, j.EmploymentType,
            j.SalaryMin, j.SalaryMax, j.CreatedDate, j.IsActive, new List<string>()
        )).ToList();

        return new RecruiterDashboardDto(
            jobs.Count(),
            apps.Count(a => a.Status == ApplicationStatus.Pending),
            apps.Count(a => a.Status == ApplicationStatus.Shortlisted),
            apps.Count(a => a.Status == ApplicationStatus.Rejected),
            activeJobDtos
        );
    }

    public async Task<CandidateDashboardDto> GetCandidateDashboardAsync(int candidateUserId)
    {
        var candidates = await _unitOfWork.Candidates.FindAsync(c => c.UserId == candidateUserId);
        var candidate = candidates.FirstOrDefault();
        if (candidate == null) throw new InvalidOperationException("Candidate profile not found.");

        var apps = (await _unitOfWork.Applications.FindAsync(a => a.CandidateId == candidate.CandidateId)).ToList();
        var appDtos = new List<ApplicationResponseDto>();
        double totalScore = 0;
        int scoreCount = 0;

        foreach (var app in apps)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(app.JobId);
            var aiMatches = await _unitOfWork.AiMatches.FindAsync(m => m.ApplicationId == app.ApplicationId);
            var aiMatch = aiMatches.FirstOrDefault();

            if (aiMatch != null)
            {
                totalScore += aiMatch.MatchScore;
                scoreCount++;
            }

            appDtos.Add(new ApplicationResponseDto(
                app.ApplicationId, app.JobId, job?.Title ?? "Position", candidate.CandidateId, "You",
                app.AppliedDate, app.Status, aiMatch?.MatchScore, null, null
            ));
        }

        double avgScore = scoreCount > 0 ? totalScore / scoreCount : 0;
        return new CandidateDashboardDto(apps.Count, Math.Round(avgScore, 1), appDtos);
    }
}
