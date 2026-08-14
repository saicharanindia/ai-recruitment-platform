using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationsController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> Apply([FromBody] ApplyJobRequestDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _applicationService.ApplyToJobAsync(userId, dto);
        return CreatedAtAction(nameof(GetApplications), new { id = result.ApplicationId }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetApplications()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var role = User.FindFirst(ClaimTypes.Role)!.Value;
        var apps = await _applicationService.GetApplicationsForUserAsync(userId, role);
        return Ok(apps);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequestDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var success = await _applicationService.UpdateApplicationStatusAsync(id, userId, dto);
        if (!success) return NotFound();
        return Ok(new { message = "Application status updated successfully." });
    }
}
