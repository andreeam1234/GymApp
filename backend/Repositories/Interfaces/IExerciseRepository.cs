using GymApp.Models;

namespace GymApp.Repositories;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<Exercise?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<Exercise>> GetByMuscleGroupAsync(string muscleGroup, CancellationToken cancellationToken = default);
}
