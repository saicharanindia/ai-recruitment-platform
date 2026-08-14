using Recruitment.Domain.Entities;

namespace Recruitment.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Candidate> Candidates { get; }
    IRepository<Recruiter> Recruiters { get; }
    IRepository<Job> Jobs { get; }
    IRepository<Resume> Resumes { get; }
    IRepository<Skill> Skills { get; }
    IRepository<Application> Applications { get; }
    IRepository<AiMatch> AiMatches { get; }
    IRepository<InterviewQuestion> InterviewQuestions { get; }

    Task<int> CompleteAsync();
}
