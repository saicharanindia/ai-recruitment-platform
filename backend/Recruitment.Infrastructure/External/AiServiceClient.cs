using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;

namespace Recruitment.Infrastructure.External;

public class AiServiceClient : IAiServiceClient
{
    private readonly HttpClient _httpClient;

    public AiServiceClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        var baseUrl = config["AI_SERVICE_URL"] ?? "http://localhost:8000";
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<ResumeParsedDataDto> ParseResumeAsync(string resumeText)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/ai/parse-resume", new { resumeText });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResumeParsedDataDto>();
                if (result != null) return result;
            }
        }
        catch { /* Fallback mock on connection error */ }

        return new ResumeParsedDataDto("Parsed Candidate", "candidate@example.com", new List<string> { "C#", ".NET Core", "Angular", "SQL Server" }, new List<string> { "5+ years in full-stack development" });
    }

    public async Task<AiMatchResultDto> ComputeMatchAsync(MatchResumeRequestDto request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/ai/match", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AiMatchResultDto>();
                if (result != null) return result;
            }
        }
        catch { /* Fallback mock */ }

        // Local calculation fallback
        var matched = request.RequiredSkills.Where(s => request.ResumeText.Contains(s, StringComparison.OrdinalIgnoreCase)).ToList();
        var missing = request.RequiredSkills.Except(matched).ToList();
        int score = request.RequiredSkills.Count > 0 ? (int)((double)matched.Count / request.RequiredSkills.Count * 100) : 80;

        return new AiMatchResultDto(score, matched, missing, score >= 75 ? "Shortlist" : "Review Required");
    }

    public async Task<List<QuestionDto>> GenerateQuestionsAsync(string jobTitle, List<string> missingSkills)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/ai/generate-questions", new { jobTitle, missingSkills });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<QuestionDto>>();
                if (result != null) return result;
            }
        }
        catch { /* Fallback mock */ }

        return new List<QuestionDto>
        {
            new QuestionDto($"Describe how you would quickly get up to speed on {string.Join(", ", missingSkills.DefaultIfEmpty("new technologies"))}.", "Adaptability"),
            new QuestionDto($"Explain core architectural concepts in {jobTitle}.", "Technical"),
            new QuestionDto("Give an example of a complex production bug you diagnosed and resolved.", "Behavioral")
        };
    }
}
