namespace Recruitment.Domain.Entities;

public class Recruiter
{
    public int RecruiterId { get; set; }
    public int UserId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

    public User User { get; set; } = null!;
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
