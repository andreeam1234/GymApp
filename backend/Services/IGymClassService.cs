using GymApp.Models;

namespace GymApp.Services;

public interface IGymClassService
{
    Task<List<GymClass>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GymClass?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<GymClass>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task AddAsync(GymClass gymClass, CancellationToken cancellationToken = default);
    Task UpdateAsync(GymClass gymClass, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<List<GymClass>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
}
