using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Interfaces;

namespace Recruitment.Application.Services;

public class JobService : IJobService
{
    private readonly IUnitOfWork _unitOfWork;

    public JobService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<JobResponseDto>> GetAllJobsAsync(string? department, string? skill)
    {
        var jobs = await _unitOfWork.Jobs.GetAllAsync();
        var activeJobs = jobs.Where(j => j.IsActive);

        if (!string.IsNullOrWhiteSpace(department))
        {
            activeJobs = activeJobs.Where(j => j.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
        }

        var resultList = new List<JobResponseDto>();
        foreach (var j in activeJobs)
        {
            var jobSkills = await _unitOfWork.Jobs.GetByIdAsync(j.JobId);
            var skills = j.JobSkills?.Select(js => js.Skill.Name).ToList() ?? new List<string>();

            if (!string.IsNullOrWhiteSpace(skill) && !skills.Any(s => s.Equals(skill, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            resultList.Add(new JobResponseDto(
                j.JobId,
                j.RecruiterId,
                j.Title,
                j.Description,
                j.Department,
                j.Location,
                j.EmploymentType,
                j.SalaryMin,
                j.SalaryMax,
                j.CreatedDate,
                j.IsActive,
                skills
            ));
        }

        return resultList;
    }

    public async Task<JobResponseDto?> GetJobByIdAsync(int jobId)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(jobId);
        if (job == null) return null;

        var skills = job.JobSkills?.Select(js => js.Skill.Name).ToList() ?? new List<string>();
        return new JobResponseDto(
            job.JobId,
            job.RecruiterId,
            job.Title,
            job.Description,
            job.Department,
            job.Location,
            job.EmploymentType,
            job.SalaryMin,
            job.SalaryMax,
            job.CreatedDate,
            job.IsActive,
            skills
        );
    }

    public async Task<JobResponseDto> CreateJobAsync(int recruiterUserId, JobCreateDto dto)
    {
        var recruiters = await _unitOfWork.Recruiters.FindAsync(r => r.UserId == recruiterUserId);
        var recruiter = recruiters.FirstOrDefault();
        if (recruiter == null) throw new InvalidOperationException("Recruiter profile not found.");

        var job = new Job
        {
            RecruiterId = recruiter.RecruiterId,
            Title = dto.Title,
            Description = dto.Description,
            Department = dto.Department,
            Location = dto.Location,
            EmploymentType = dto.EmploymentType,
            SalaryMin = dto.SalaryMin,
            SalaryMax = dto.SalaryMax,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        await _unitOfWork.Jobs.AddAsync(job);
        await _unitOfWork.CompleteAsync();

        if (dto.RequiredSkills != null && dto.RequiredSkills.Any())
        {
            var allSkills = await _unitOfWork.Skills.GetAllAsync();
            foreach (var skillName in dto.RequiredSkills)
            {
                var existingSkill = allSkills.FirstOrDefault(s => s.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase));
                if (existingSkill == null)
                {
                    existingSkill = new Skill { Name = skillName };
                    await _unitOfWork.Skills.AddAsync(existingSkill);
                    await _unitOfWork.CompleteAsync();
                }

                job.JobSkills.Add(new JobSkill { JobId = job.JobId, SkillId = existingSkill.SkillId });
            }
            await _unitOfWork.CompleteAsync();
        }

        return new JobResponseDto(
            job.JobId,
            job.RecruiterId,
            job.Title,
            job.Description,
            job.Department,
            job.Location,
            job.EmploymentType,
            job.SalaryMin,
            job.SalaryMax,
            job.CreatedDate,
            job.IsActive,
            dto.RequiredSkills ?? new List<string>()
        );
    }

    public async Task<bool> UpdateJobAsync(int jobId, int recruiterUserId, JobCreateDto dto)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(jobId);
        if (job == null) return false;

        job.Title = dto.Title;
        job.Description = dto.Description;
        job.Department = dto.Department;
        job.Location = dto.Location;
        job.EmploymentType = dto.EmploymentType;
        job.SalaryMin = dto.SalaryMin;
        job.SalaryMax = dto.SalaryMax;

        _unitOfWork.Jobs.Update(job);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> DeleteJobAsync(int jobId, int recruiterUserId)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(jobId);
        if (job == null) return false;

        job.IsActive = false;
        _unitOfWork.Jobs.Update(job);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
