using Moq;
using Recruitment.Application.DTOs;
using Recruitment.Application.Services;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Interfaces;
using Xunit;

namespace Recruitment.Tests.Services;

public class JobServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly JobService _jobService;

    public JobServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _jobService = new JobService(_mockUow.Object);
    }

    [Fact]
    public async Task GetAllJobsAsync_ReturnsActiveJobsOnly()
    {
        // Arrange
        var mockJobs = new List<Job>
        {
            new Job { JobId = 1, Title = ".NET Engineer", IsActive = true, Department = "Engineering" },
            new Job { JobId = 2, Title = "QA Lead", IsActive = false, Department = "QA" }
        };

        var mockRepo = new Mock<IRepository<Job>>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(mockJobs);
        _mockUow.Setup(u => u.Jobs).Returns(mockRepo.Object);

        // Act
        var result = await _jobService.GetAllJobsAsync(null, null);

        // Assert
        Assert.Single(result);
        Assert.Equal(".NET Engineer", result.First().Title);
    }
}
