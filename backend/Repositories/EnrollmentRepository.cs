using GymApp.Data;
using GymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repositories;

public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(AppDbContext context) : base(context) { }

    public async Task<List<Enrollment>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
        => await _context.Enrollments
            .Where(e => e.UserId == userId)
            .Include(e => e.ClassSession).ThenInclude(cs => cs.GymClass)
            .OrderByDescending(e => e.EnrolledAt)
            .ToListAsync(cancellationToken);

    public async Task<List<Enrollment>> GetBySessionAsync(int sessionId, CancellationToken cancellationToken = default)
        => await _context.Enrollments
            .Where(e => e.ClassSessionId == sessionId)
            .Include(e => e.User)
            .ToListAsync(cancellationToken);

    public async Task<Enrollment?> GetByUserAndSessionAsync(string userId, int sessionId, CancellationToken cancellationToken = default)
        => await _context.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.ClassSessionId == sessionId, cancellationToken);

    public async Task<int> CountActiveBySessionAsync(int sessionId, CancellationToken cancellationToken = default)
        => await _context.Enrollments
            .CountAsync(e => e.ClassSessionId == sessionId && e.Status == EnrollmentStatus.Confirmed, cancellationToken);
}
