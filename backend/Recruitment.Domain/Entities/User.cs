using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities;

public class User
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Candidate? CandidateProfile { get; set; }
    public Recruiter? RecruiterProfile { get; set; }
}
