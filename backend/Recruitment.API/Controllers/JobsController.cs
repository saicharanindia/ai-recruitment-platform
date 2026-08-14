using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs([FromQuery] string? department, [FromQuery] string? skill)
    {
        var jobs = await _jobService.GetAllJobsAsync(department, skill);
        return Ok(jobs);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetJob(int id)
    {
        var job = await _jobService.GetJobByIdAsync(id);
        if (job == null) return NotFound("Job not found.");
        return Ok(job);
    }

    [HttpPost]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> CreateJob([FromBody] JobCreateDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var job = await _jobService.CreateJobAsync(userId, dto);
        return CreatedAtAction(nameof(GetJob), new { id = job.JobId }, job);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> UpdateJob(int id, [FromBody] JobCreateDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var success = await _jobService.UpdateJobAsync(id, userId, dto);
        if (!success) return NotFound();
        return Ok(new { message = "Job updated successfully." });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Recruiter")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var success = await _jobService.DeleteJobAsync(id, userId);
        if (!success) return NotFound();
        return NoContent();
    }
}
