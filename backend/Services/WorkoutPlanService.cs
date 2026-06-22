using GymApp.Models;
using GymApp.Repositories;

namespace GymApp.Services;

public class WorkoutPlanService : IWorkoutPlanService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkoutPlanService> _logger;

    public WorkoutPlanService(IUnitOfWork unitOfWork, ILogger<WorkoutPlanService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<WorkoutPlan>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.WorkoutPlanRepository.GetAllWithDetailsAsync(cancellationToken);

    public async Task<WorkoutPlan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _unitOfWork.WorkoutPlanRepository.GetByIdWithDetailsAsync(id, cancellationToken);

    public async Task<List<WorkoutPlan>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
        => await _unitOfWork.WorkoutPlanRepository.GetByUserAsync(userId, cancellationToken);

    public async Task AddAsync(WorkoutPlan plan, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating workout plan {Title} for user {UserId}", plan.Title, plan.UserId);
        await _unitOfWork.WorkoutPlanRepository.AddAsync(plan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Workout plan created with id {PlanId}", plan.Id);
    }

    public async Task UpdateAsync(WorkoutPlan plan, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating workout plan {PlanId}", plan.Id);
        _unitOfWork.WorkoutPlanRepository.Update(plan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _unitOfWork.WorkoutPlanRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"WorkoutPlan {id} not found");

        _logger.LogInformation("Deleting workout plan {PlanId}", id);
        _unitOfWork.WorkoutPlanRepository.Delete(plan);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
