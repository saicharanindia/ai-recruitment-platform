using Recruitment.Domain.Entities;

namespace Recruitment.Application.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

public interface IAiServiceClient
{
    Task<ResumeParsedDataDto> ParseResumeAsync(string resumeText);
    Task<AiMatchResultDto> ComputeMatchAsync(MatchResumeRequestDto request);
    Task<List<QuestionDto>> GenerateQuestionsAsync(string jobTitle, List<string> missingSkills);
}
