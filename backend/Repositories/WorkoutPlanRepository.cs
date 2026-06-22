using GymApp.Data;
using GymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repositories;

public class WorkoutPlanRepository : Repository<WorkoutPlan>, IWorkoutPlanRepository
{
    public WorkoutPlanRepository(AppDbContext context) : base(context) { }

    public async Task<List<WorkoutPlan>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(wp => wp.User)
            .Include(wp => wp.WorkoutPlanExercises)
                .ThenInclude(wpe => wpe.Exercise)
            .OrderByDescending(wp => wp.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkoutPlan?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(wp => wp.User)
            .Include(wp => wp.WorkoutPlanExercises)
                .ThenInclude(wpe => wpe.Exercise)
            .FirstOrDefaultAsync(wp => wp.Id == id, cancellationToken);
    }

    public async Task<List<WorkoutPlan>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Where(wp => wp.UserId == userId)
            .Include(wp => wp.WorkoutPlanExercises)
                .ThenInclude(wpe => wpe.Exercise)
            .OrderByDescending(wp => wp.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
