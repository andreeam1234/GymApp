using GymApp.Models;

namespace GymApp.Services;

public interface IWorkoutPlanService
{
    Task<List<WorkoutPlan>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<WorkoutPlan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<WorkoutPlan>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task AddAsync(WorkoutPlan plan, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkoutPlan plan, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
