namespace Recruitment.Domain.Entities;

public class Job
{
    public int JobId { get; set; }
    public int RecruiterId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = "Full-time";
    public double? SalaryMin { get; set; }
    public double? SalaryMax { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ClosingDate { get; set; }
    public bool IsActive { get; set; } = true;

    public Recruiter Recruiter { get; set; } = null!;
    public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}
