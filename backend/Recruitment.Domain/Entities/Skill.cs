namespace Recruitment.Domain.Entities;

public class Skill
{
    public int SkillId { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
    public ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
}

public class CandidateSkill
{
    public int CandidateSkillId { get; set; }
    public int CandidateId { get; set; }
    public int SkillId { get; set; }

    public Candidate Candidate { get; set; } = null!;
    public Skill Skill { get; set; } = null!;
}

public class JobSkill
{
    public int JobSkillId { get; set; }
    public int JobId { get; set; }
    public int SkillId { get; set; }

    public Job Job { get; set; } = null!;
    public Skill Skill { get; set; } = null!;
}
