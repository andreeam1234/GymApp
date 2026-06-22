using GymApp.Models;

namespace GymApp.Services;

public interface IEnrollmentService
{
    Task<List<Enrollment>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<List<Enrollment>> GetBySessionAsync(int sessionId, CancellationToken cancellationToken = default);
    Task<Enrollment> EnrollAsync(string userId, int sessionId, CancellationToken cancellationToken = default);
    Task CancelAsync(string userId, int enrollmentId, CancellationToken cancellationToken = default);
}
