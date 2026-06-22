using GymApp.Models;

namespace GymApp.Repositories;

public interface IClassSessionRepository : IRepository<ClassSession>
{
    Task<List<ClassSession>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<ClassSession?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<List<ClassSession>> GetUpcomingAsync(CancellationToken cancellationToken = default);
    Task<List<ClassSession>> GetByTrainerAsync(string trainerId, CancellationToken cancellationToken = default);
}
