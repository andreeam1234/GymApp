using GymApp.Data;
using GymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repositories;

public class ClassSessionRepository : Repository<ClassSession>, IClassSessionRepository
{
    public ClassSessionRepository(AppDbContext context) : base(context) { }

    public async Task<List<ClassSession>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ClassSessions
            .Include(cs => cs.GymClass)
            .Include(cs => cs.Trainer)
            .Include(cs => cs.Enrollments)
            .OrderBy(cs => cs.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<ClassSession?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ClassSessions
            .Include(cs => cs.GymClass)
            .Include(cs => cs.Trainer)
            .Include(cs => cs.Enrollments)
                .ThenInclude(e => e.User)
            .FirstOrDefaultAsync(cs => cs.Id == id, cancellationToken);
    }

    public async Task<List<ClassSession>> GetUpcomingAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ClassSessions
            .Where(cs => cs.StartTime >= DateTime.UtcNow)
            .Include(cs => cs.GymClass)
            .Include(cs => cs.Trainer)
            .Include(cs => cs.Enrollments)
            .OrderBy(cs => cs.StartTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ClassSession>> GetByTrainerAsync(string trainerId, CancellationToken cancellationToken = default)
    {
        return await _context.ClassSessions
            .Where(cs => cs.TrainerId == trainerId)
            .Include(cs => cs.GymClass)
            .Include(cs => cs.Enrollments)
            .OrderBy(cs => cs.StartTime)
            .ToListAsync(cancellationToken);
    }
}
