using GymApp.Data;
using GymApp.Models;
using GymApp.Repositories;
using GymApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GymApp.Tests.Services;

public class EnrollmentServiceTests
{
    // Helper mic — creează un context nou, cu un GymClass + un Trainer + o
    // ClassSession deja seed-uite, ca fiecare test să nu repete aceleași 15 linii.
    private static (AppDbContext context, EnrollmentService service, ClassSession session) Setup(int capacity)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);

        var gymClass = new GymClass { Id = 1, Name = "Yoga", Description = "Relaxing class", DurationMinutes = 60, Category = "Wellness" };
        var trainer = new ApplicationUser { Id = "trainer-1", UserName = "trainer@gym.com", Email = "trainer@gym.com", FirstName = "Ana", LastName = "Pop" };
        var session = new ClassSession
        {
            Id = 1,
            GymClassId = 1,
            TrainerId = "trainer-1",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            Capacity = capacity,
            Room = "Room 1"
        };

        context.GymClasses.Add(gymClass);
        context.Users.Add(trainer);
        context.ClassSessions.Add(session);
        context.SaveChanges();

        var unitOfWork = new UnitOfWork(context);
        var service = new EnrollmentService(unitOfWork, Mock.Of<ILogger<EnrollmentService>>());

        return (context, service, session);
    }

    [Fact]
    public async Task EnrollAsync_ValidRequest_CreatesConfirmedEnrollment()
    {
        // Arrange
        var (_, service, session) = Setup(capacity: 5);

        // Act
        var enrollment = await service.EnrollAsync("user-1", session.Id);

        // Assert
        Assert.Equal(EnrollmentStatus.Confirmed, enrollment.Status);
    }

    [Fact]
    public async Task EnrollAsync_SessionAtCapacity_ThrowsInvalidOperationException()
    {
        // Arrange — capacitate 1, deja ocupată de un prim user
        var (_, service, session) = Setup(capacity: 1);
        await service.EnrollAsync("user-existing", session.Id);

        // Act + Assert — al doilea user trebuie respins
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.EnrollAsync("user-new", session.Id));
    }

    [Fact]
    public async Task EnrollAsync_UserAlreadyEnrolled_ThrowsArgumentException()
    {
        // Arrange
        var (_, service, session) = Setup(capacity: 10);
        await service.EnrollAsync("user-1", session.Id);

        // Act + Assert — același user, aceeași sesiune, a doua oară
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.EnrollAsync("user-1", session.Id));
    }

    [Fact]
    public async Task EnrollAsync_SessionDoesNotExist_ThrowsKeyNotFoundException()
    {
        var (_, service, _) = Setup(capacity: 5);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.EnrollAsync("user-1", sessionId: 999));
    }

    [Fact]
    public async Task CancelAsync_AnotherUsersEnrollment_ThrowsUnauthorizedAccessException()
    {
        // Arrange — "user-1" se înscrie, "user-2" încearcă să anuleze
        var (_, service, session) = Setup(capacity: 5);
        var enrollment = await service.EnrollAsync("user-1", session.Id);

        // Act + Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => service.CancelAsync("user-2", enrollment.Id));
    }
}