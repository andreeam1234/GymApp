using GymApp.Models;

namespace GymApp.Services;

public interface IClassSessionService
{
    Task<List<ClassSession>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClassSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<ClassSession>> GetUpcomingAsync(CancellationToken cancellationToken = default);
    Task<List<ClassSession>> GetByTrainerAsync(string trainerId, CancellationToken cancellationToken = default);
    Task AddAsync(ClassSession session, CancellationToken cancellationToken = default);
    Task UpdateAsync(ClassSession session, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
