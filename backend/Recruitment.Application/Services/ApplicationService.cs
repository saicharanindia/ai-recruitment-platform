using System.Text.Json;
using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Interfaces;

namespace Recruitment.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAiServiceClient _aiServiceClient;

    public ApplicationService(IUnitOfWork unitOfWork, IAiServiceClient aiServiceClient)
    {
        _unitOfWork = unitOfWork;
        _aiServiceClient = aiServiceClient;
    }

    public async Task<ApplicationResponseDto> ApplyToJobAsync(int candidateUserId, ApplyJobRequestDto dto)
    {
        var candidates = await _unitOfWork.Candidates.FindAsync(c => c.UserId == candidateUserId);
        var candidate = candidates.FirstOrDefault();
        if (candidate == null) throw new InvalidOperationException("Candidate profile not found.");

        var existingApps = await _unitOfWork.Applications.FindAsync(a => a.CandidateId == candidate.CandidateId && a.JobId == dto.JobId);
        if (existingApps.Any())
        {
            throw new InvalidOperationException("You have already submitted an application for this job.");
        }

        var job = await _unitOfWork.Jobs.GetByIdAsync(dto.JobId);
        if (job == null || !job.IsActive) throw new KeyNotFoundException("Job posting not found or no longer active.");

        var candidateUser = await _unitOfWork.Users.GetByIdAsync(candidateUserId);

        var application = new Application
        {
            JobId = dto.JobId,
            CandidateId = candidate.CandidateId,
            AppliedDate = DateTime.UtcNow,
            Status = ApplicationStatus.Pending,
            ResumeAtApply = candidate.ResumeLink
        };

        await _unitOfWork.Applications.AddAsync(application);
        await _unitOfWork.CompleteAsync();

        // Perform AI matching trigger automatically
        var jobSkills = job.JobSkills?.Select(js => js.Skill.Name).ToList() ?? new List<string>();
        var resumeText = $"Candidate {candidateUser?.FullName} Resume Content: Experienced in software development, C#, .NET, Python, Angular.";
        
        var matchRequest = new MatchResumeRequestDto(resumeText, job.Description, jobSkills);
        var matchResult = await _aiServiceClient.ComputeMatchAsync(matchRequest);

        var aiMatch = new AiMatch
        {
            ApplicationId = application.ApplicationId,
            MatchScore = matchResult.MatchScore,
            MatchedSkills = JsonSerializer.Serialize(matchResult.MatchedSkills),
            MissingSkills = JsonSerializer.Serialize(matchResult.MissingSkills),
            CalculatedAt = DateTime.UtcNow
        };

        await _unitOfWork.AiMatches.AddAsync(aiMatch);
        await _unitOfWork.CompleteAsync();

        return new ApplicationResponseDto(
            application.ApplicationId,
            job.JobId,
            job.Title,
            candidate.CandidateId,
            candidateUser?.FullName ?? "Candidate",
            application.AppliedDate,
            application.Status,
            matchResult.MatchScore,
            matchResult.MatchedSkills,
            matchResult.MissingSkills
        );
    }

    public async Task<IEnumerable<ApplicationResponseDto>> GetApplicationsForUserAsync(int userId, string role)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return Enumerable.Empty<ApplicationResponseDto>();

        var allApps = await _unitOfWork.Applications.GetAllAsync();
        var result = new List<ApplicationResponseDto>();

        if (role == UserRole.Candidate.ToString())
        {
            var candidates = await _unitOfWork.Candidates.FindAsync(c => c.UserId == userId);
            var candidate = candidates.FirstOrDefault();
            if (candidate == null) return Enumerable.Empty<ApplicationResponseDto>();

            allApps = allApps.Where(a => a.CandidateId == candidate.CandidateId);
        }
        else if (role == UserRole.Recruiter.ToString())
        {
            var recruiters = await _unitOfWork.Recruiters.FindAsync(r => r.UserId == userId);
            var recruiter = recruiters.FirstOrDefault();
            if (recruiter == null) return Enumerable.Empty<ApplicationResponseDto>();

            var recruiterJobIds = (await _unitOfWork.Jobs.FindAsync(j => j.RecruiterId == recruiter.RecruiterId)).Select(j => j.JobId).ToHashSet();
            allApps = allApps.Where(a => recruiterJobIds.Contains(a.JobId));
        }

        foreach (var app in allApps)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(app.JobId);
            var candidateObj = await _unitOfWork.Candidates.GetByIdAsync(app.CandidateId);
            var candidateUserObj = candidateObj != null ? await _unitOfWork.Users.GetByIdAsync(candidateObj.UserId) : null;
            var aiMatches = await _unitOfWork.AiMatches.FindAsync(m => m.ApplicationId == app.ApplicationId);
            var aiMatch = aiMatches.FirstOrDefault();

            List<string>? matched = aiMatch?.MatchedSkills != null ? JsonSerializer.Deserialize<List<string>>(aiMatch.MatchedSkills) : null;
            List<string>? missing = aiMatch?.MissingSkills != null ? JsonSerializer.Deserialize<List<string>>(aiMatch.MissingSkills) : null;

            result.Add(new ApplicationResponseDto(
                app.ApplicationId,
                app.JobId,
                job?.Title ?? "Position",
                app.CandidateId,
                candidateUserObj?.FullName ?? "Candidate",
                app.AppliedDate,
                app.Status,
                aiMatch?.MatchScore,
                matched,
                missing
            ));
        }

        return result;
    }

    public async Task<bool> UpdateApplicationStatusAsync(int applicationId, int recruiterUserId, UpdateStatusRequestDto dto)
    {
        var app = await _unitOfWork.Applications.GetByIdAsync(applicationId);
        if (app == null) return false;

        app.Status = dto.Status;
        _unitOfWork.Applications.Update(app);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
