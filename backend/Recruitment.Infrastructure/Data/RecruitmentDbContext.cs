using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;

namespace Recruitment.Infrastructure.Data;

public class RecruitmentDbContext : DbContext
{
    public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Recruiter> Recruiters => Set<Recruiter>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();
    public DbSet<JobSkill> JobSkills => Set<JobSkill>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<AiMatch> AiMatches => Set<AiMatch>();
    public DbSet<InterviewQuestion> InterviewQuestions => Set<InterviewQuestion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Enums
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        // Application Enum
        modelBuilder.Entity<Application>()
            .Property(a => a.Status)
            .HasConversion<string>();

        // Unique constraints
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Skill>().HasIndex(s => s.Name).IsUnique();
        modelBuilder.Entity<CandidateSkill>().HasIndex(cs => new { cs.CandidateId, cs.SkillId }).IsUnique();
        modelBuilder.Entity<JobSkill>().HasIndex(js => new { js.JobId, js.SkillId }).IsUnique();
        modelBuilder.Entity<Application>().HasIndex(a => new { a.CandidateId, a.JobId }).IsUnique();

        // Foreign Key Relationships
        modelBuilder.Entity<User>()
            .HasOne(u => u.CandidateProfile)
            .WithOne(c => c.User)
            .HasForeignKey<Candidate>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.RecruiterProfile)
            .WithOne(r => r.User)
            .HasForeignKey<Recruiter>(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
