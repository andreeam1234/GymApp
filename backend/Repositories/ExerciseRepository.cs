using GymApp.Data;
using GymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repositories;

public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(AppDbContext context) : base(context) { }

    public async Task<Exercise?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _context.Exercises.FirstOrDefaultAsync(e => e.Name == name, cancellationToken);

    public async Task<List<Exercise>> GetByMuscleGroupAsync(string muscleGroup, CancellationToken cancellationToken = default)
        => await _context.Exercises
            .Where(e => e.MuscleGroup == muscleGroup)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
}
