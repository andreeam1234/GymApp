using GymApp.Models;
using GymApp.Repositories;

namespace GymApp.Services;

public class ExerciseService : IExerciseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExerciseService> _logger;

    public ExerciseService(IUnitOfWork unitOfWork, ILogger<ExerciseService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<Exercise>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.ExerciseRepository.GetAllAsync(cancellationToken);

    public async Task<Exercise?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _unitOfWork.ExerciseRepository.GetByIdAsync(id, cancellationToken);

    public async Task<List<Exercise>> GetByMuscleGroupAsync(string muscleGroup, CancellationToken cancellationToken = default)
        => await _unitOfWork.ExerciseRepository.GetByMuscleGroupAsync(muscleGroup, cancellationToken);

    public async Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.ExerciseRepository.GetByNameAsync(exercise.Name, cancellationToken);
        if (existing != null)
            throw new ArgumentException($"Exercise '{exercise.Name}' already exists");

        _logger.LogInformation("Creating exercise {Name}", exercise.Name);
        await _unitOfWork.ExerciseRepository.AddAsync(exercise, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Exercise exercise, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating exercise {ExerciseId}", exercise.Id);
        _unitOfWork.ExerciseRepository.Update(exercise);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var exercise = await _unitOfWork.ExerciseRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Exercise {id} not found");

        _logger.LogInformation("Deleting exercise {ExerciseId}", id);
        _unitOfWork.ExerciseRepository.Delete(exercise);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
