namespace Recruitment.Domain.Entities;

public class Candidate
{
    public int CandidateId { get; set; }
    public int UserId { get; set; }
    public string? Phone { get; set; }
    public string? ResumeLink { get; set; }
    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ICollection<Resume> Resumes { get; set; } = new List<Resume>();
    public ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}
