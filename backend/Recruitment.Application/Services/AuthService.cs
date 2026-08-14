using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Interfaces;

namespace Recruitment.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUsers = await _unitOfWork.Users.FindAsync(u => u.Email == request.Email);
        if (existingUsers.Any())
        {
            throw new InvalidOperationException("Email address is already registered.");
        }

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = request.Role,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CompleteAsync();

        if (request.Role == UserRole.Candidate)
        {
            var candidate = new Candidate
            {
                UserId = user.UserId,
                Phone = request.Phone,
                JoinedDate = DateTime.UtcNow
            };
            await _unitOfWork.Candidates.AddAsync(candidate);
        }
        else if (request.Role == UserRole.Recruiter)
        {
            var recruiter = new Recruiter
            {
                UserId = user.UserId,
                Department = request.Department ?? "General",
                Title = request.Title ?? "Recruiter"
            };
            await _unitOfWork.Recruiters.AddAsync(recruiter);
        }

        await _unitOfWork.CompleteAsync();

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthResponseDto(user.UserId, user.FullName, user.Email, user.Role.ToString(), token, 3600);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var users = await _unitOfWork.Users.FindAsync(u => u.Email == request.Email);
        var user = users.FirstOrDefault();
        if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthResponseDto(user.UserId, user.FullName, user.Email, user.Role.ToString(), token, 3600);
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return null;

        string? phone = null, department = null, title = null;
        if (user.Role == UserRole.Candidate)
        {
            var candidates = await _unitOfWork.Candidates.FindAsync(c => c.UserId == userId);
            phone = candidates.FirstOrDefault()?.Phone;
        }
        else if (user.Role == UserRole.Recruiter)
        {
            var recruiters = await _unitOfWork.Recruiters.FindAsync(r => r.UserId == userId);
            var recruiter = recruiters.FirstOrDefault();
            department = recruiter?.Department;
            title = recruiter?.Title;
        }

        return new UserProfileDto(user.UserId, user.FullName, user.Email, user.Role.ToString(), phone, department, title);
    }
}
