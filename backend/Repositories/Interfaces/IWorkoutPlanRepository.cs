using GymApp.Models;

namespace GymApp.Repositories;

public interface IWorkoutPlanRepository : IRepository<WorkoutPlan>
{
    Task<List<WorkoutPlan>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<WorkoutPlan?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<List<WorkoutPlan>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
}
