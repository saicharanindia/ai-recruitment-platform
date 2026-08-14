namespace Recruitment.Domain.Entities;

public class Resume
{
    public int ResumeId { get; set; }
    public int CandidateId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Candidate Candidate { get; set; } = null!;
}
