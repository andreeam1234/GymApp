using GymApp.Models;

namespace GymApp.Services;

public interface IExerciseService
{
    Task<List<Exercise>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Exercise?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Exercise>> GetByMuscleGroupAsync(string muscleGroup, CancellationToken cancellationToken = default);
    Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default);
    Task UpdateAsync(Exercise exercise, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
