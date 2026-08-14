using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Interfaces;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly RecruitmentDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(RecruitmentDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Remove(T entity) => _dbSet.Remove(entity);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly RecruitmentDbContext _context;

    public UnitOfWork(RecruitmentDbContext context)
    {
        _context = context;
        Users = new Repository<Domain.Entities.User>(_context);
        Candidates = new Repository<Domain.Entities.Candidate>(_context);
        Recruiters = new Repository<Domain.Entities.Recruiter>(_context);
        Jobs = new Repository<Domain.Entities.Job>(_context);
        Resumes = new Repository<Domain.Entities.Resume>(_context);
        Skills = new Repository<Domain.Entities.Skill>(_context);
        Applications = new Repository<Domain.Entities.Application>(_context);
        AiMatches = new Repository<Domain.Entities.AiMatch>(_context);
        InterviewQuestions = new Repository<Domain.Entities.InterviewQuestion>(_context);
    }

    public IRepository<Domain.Entities.User> Users { get; }
    public IRepository<Domain.Entities.Candidate> Candidates { get; }
    public IRepository<Domain.Entities.Recruiter> Recruiters { get; }
    public IRepository<Domain.Entities.Job> Jobs { get; }
    public IRepository<Domain.Entities.Resume> Resumes { get; }
    public IRepository<Domain.Entities.Skill> Skills { get; }
    public IRepository<Domain.Entities.Application> Applications { get; }
    public IRepository<Domain.Entities.AiMatch> AiMatches { get; }
    public IRepository<Domain.Entities.InterviewQuestion> InterviewQuestions { get; }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
