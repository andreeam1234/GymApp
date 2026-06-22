using GymApp.Models;

namespace GymApp.Services;

public interface IMembershipService
{
    Task<List<Membership>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Membership?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Membership>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<Membership?> GetActiveByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task AddAsync(Membership membership, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
