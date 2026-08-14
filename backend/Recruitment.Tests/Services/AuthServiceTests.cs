using Moq;
using Recruitment.Application.DTOs;
using Recruitment.Application.Interfaces;
using Recruitment.Application.Services;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Interfaces;
using Xunit;

namespace Recruitment.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IPasswordHasher> _mockHasher;
    private readonly Mock<IJwtTokenGenerator> _mockJwt;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockHasher = new Mock<IPasswordHasher>();
        _mockJwt = new Mock<IJwtTokenGenerator>();

        _authService = new AuthService(_mockUow.Object, _mockHasher.Object, _mockJwt.Object);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsAuthResponse()
    {
        // Arrange
        var testUser = new User
        {
            UserId = 1,
            Email = "test@example.com",
            FullName = "Test User",
            PasswordHash = "hashedPassword",
            Role = UserRole.Candidate
        };

        var mockUserRepo = new Mock<IRepository<User>>();
        mockUserRepo.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                    .ReturnsAsync(new List<User> { testUser });

        _mockUow.Setup(u => u.Users).Returns(mockUserRepo.Object);
        _mockHasher.Setup(h => h.VerifyPassword("password123", "hashedPassword")).Returns(true);
        _mockJwt.Setup(j => j.GenerateToken(testUser)).Returns("jwt_token_sample");

        var loginDto = new LoginRequestDto("test@example.com", "password123");

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("jwt_token_sample", result.Token);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var testUser = new User
        {
            UserId = 1,
            Email = "test@example.com",
            PasswordHash = "hashedPassword",
            Role = UserRole.Candidate
        };

        var mockUserRepo = new Mock<IRepository<User>>();
        mockUserRepo.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>()))
                    .ReturnsAsync(new List<User> { testUser });

        _mockUow.Setup(u => u.Users).Returns(mockUserRepo.Object);
        _mockHasher.Setup(h => h.VerifyPassword("wrongpass", "hashedPassword")).Returns(false);

        var loginDto = new LoginRequestDto("test@example.com", "wrongpass");

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDto));
    }
}
