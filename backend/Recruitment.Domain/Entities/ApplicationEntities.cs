using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities;

public class Application
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }
    public int CandidateId { get; set; }
    public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    public string? ResumeAtApply { get; set; }

    public Job Job { get; set; } = null!;
    public Candidate Candidate { get; set; } = null!;
    public AiMatch? AiMatch { get; set; }
    public ICollection<InterviewQuestion> InterviewQuestions { get; set; } = new List<InterviewQuestion>();
}

public class AiMatch
{
    public int MatchId { get; set; }
    public int ApplicationId { get; set; }
    public int MatchScore { get; set; }
    public string? MatchedSkills { get; set; } // JSON text array
    public string? MissingSkills { get; set; } // JSON text array
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;

    public Application Application { get; set; } = null!;
}

public class InterviewQuestion
{
    public int QuestionId { get; set; }
    public int ApplicationId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string Category { get; set; } = "Technical";

    public Application Application { get; set; } = null!;
}
