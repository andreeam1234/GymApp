using GymApp.Data;
using GymApp.Models;
using GymApp.Repositories;
using GymApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GymApp.Tests.Services;

public class MembershipServiceTests
{
    private static MembershipService CreateService()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        return new MembershipService(unitOfWork, Mock.Of<ILogger<MembershipService>>());
    }

    [Fact]
    public async Task AddAsync_EndDateBeforeStartDate_ThrowsArgumentException()
    {
        // Arrange — regula exactă din MembershipService.AddAsync:
        // "if (membership.EndDate <= membership.StartDate) throw ..."
        var service = CreateService();
        var membership = new Membership
        {
            UserId = "user-1",
            Type = MembershipType.Basic,
            Price = 100,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(-1) // se termină înainte să înceapă
        };

        // Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.AddAsync(membership));
    }

    [Fact]
    public async Task AddAsync_ValidDates_Succeeds()
    {
        // Arrange
        var service = CreateService();
        var membership = new Membership
        {
            UserId = "user-1",
            Type = MembershipType.Standard,
            Price = 150,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(1)
        };

        // Act
        await service.AddAsync(membership);

        // Assert
        var all = await service.GetAllAsync();
        Assert.Single(all);
    }
}