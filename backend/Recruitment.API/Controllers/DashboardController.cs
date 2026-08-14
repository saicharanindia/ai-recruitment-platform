using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IAiService _aiService;

    public DashboardController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpGet("recruiter")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> GetRecruiterDashboard()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var stats = await _aiService.GetRecruiterDashboardAsync(userId);
        return Ok(stats);
    }

    [HttpGet("candidate")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> GetCandidateDashboard()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var stats = await _aiService.GetCandidateDashboardAsync(userId);
        return Ok(stats);
    }
}
