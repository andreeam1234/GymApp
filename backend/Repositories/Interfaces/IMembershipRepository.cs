using GymApp.Models;

namespace GymApp.Repositories;

public interface IMembershipRepository : IRepository<Membership>
{
    Task<List<Membership>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<Membership?> GetActiveByUserAsync(string userId, CancellationToken cancellationToken = default);
}
