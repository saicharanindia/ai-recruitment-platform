using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("analyze-resume")]
    public async Task<IActionResult> AnalyzeResume([FromBody] AnalyzeResumeRequestDto dto)
    {
        var result = await _aiService.AnalyzeResumeAsync(dto.ResumeText);
        return Ok(result);
    }

    [HttpPost("match-resume")]
    public async Task<IActionResult> MatchResume([FromBody] MatchResumeRequestDto dto)
    {
        var result = await _aiService.MatchResumeToJobAsync(dto);
        return Ok(result);
    }

    [HttpPost("generate-questions")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> GenerateInterviewQuestions([FromBody] GenerateQuestionsRequestDto dto)
    {
        var questions = await _aiService.GenerateInterviewQuestionsAsync(dto.ApplicationId);
        return Ok(questions);
    }
}
