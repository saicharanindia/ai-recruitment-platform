using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;

namespace Recruitment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResumesController : ControllerBase
{
    private readonly IAiService _aiService;
    private readonly IWebHostEnvironment _env;

    public ResumesController(IAiService aiService, IWebHostEnvironment env)
    {
        _aiService = aiService;
        _env = env;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Candidate")]
    public async Task<IActionResult> UploadResume(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("File is required.");
        if (Path.GetExtension(file.FileName).ToLower() is not (".pdf" or ".docx" or ".txt"))
            return BadRequest("Only .pdf, .docx, and .txt files are supported.");

        var uploadsFolder = Path.Combine(_env.ContentRootPath, "uploads", "resumes");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var resumeId = Random.Shared.Next(100, 999);
        return Created("", new ResumeUploadResponseDto(resumeId, $"uploads/resumes/{uniqueFileName}", DateTime.UtcNow));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetParsedResume(int id)
    {
        var mockText = "Candidate Sample Resume Text: Expertise in C#, .NET 10, Angular 22, SQL Server, Docker.";
        var parsed = await _aiService.AnalyzeResumeAsync(mockText);
        return Ok(parsed);
    }
}
