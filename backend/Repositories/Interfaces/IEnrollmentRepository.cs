using GymApp.Models;

namespace GymApp.Repositories;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<List<Enrollment>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<List<Enrollment>> GetBySessionAsync(int sessionId, CancellationToken cancellationToken = default);
    Task<Enrollment?> GetByUserAndSessionAsync(string userId, int sessionId, CancellationToken cancellationToken = default);
    Task<int> CountActiveBySessionAsync(int sessionId, CancellationToken cancellationToken = default);
}
