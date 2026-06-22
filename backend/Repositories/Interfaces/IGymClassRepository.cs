using GymApp.Models;

namespace GymApp.Repositories;

public interface IGymClassRepository : IRepository<GymClass>
{
    Task<List<GymClass>> GetAllWithSessionsAsync(CancellationToken cancellationToken = default);
    Task<GymClass?> GetByIdWithSessionsAsync(int id, CancellationToken cancellationToken = default);
    Task<List<GymClass>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<List<GymClass>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
