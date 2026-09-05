using GymApp.Data;
using GymApp.Models;
using GymApp.Repositories;
using GymApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GymApp.Tests.Services;

public class ExerciseServiceTests
{
    [Fact]
    public async Task AddAsync_NewName_Succeeds()
    {
        // Arrange — o bază de date falsă, în memorie, unică pentru testul ăsta
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var service = new ExerciseService(unitOfWork, Mock.Of<ILogger<ExerciseService>>());

        // Act
        await service.AddAsync(new Exercise { Name = "Squat", MuscleGroup = "Legs" });

        // Assert
        var all = await service.GetAllAsync();
        Assert.Single(all);
    }

    [Fact]
    public async Task AddAsync_DuplicateName_ThrowsArgumentException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var service = new ExerciseService(unitOfWork, Mock.Of<ILogger<ExerciseService>>());
        await service.AddAsync(new Exercise { Name = "Squat", MuscleGroup = "Legs" });

        // Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.AddAsync(new Exercise { Name = "Squat", MuscleGroup = "Glutes" }));
    }
}