using GymApp.Data;
using GymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repositories;

public class MembershipRepository : Repository<Membership>, IMembershipRepository
{
    public MembershipRepository(AppDbContext context) : base(context) { }

    public async Task<List<Membership>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
        => await _context.Memberships
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.StartDate)
            .ToListAsync(cancellationToken);

    public async Task<Membership?> GetActiveByUserAsync(string userId, CancellationToken cancellationToken = default)
        => await _context.Memberships
            .Where(m => m.UserId == userId && m.IsActive && m.EndDate >= DateTime.UtcNow)
            .OrderByDescending(m => m.StartDate)
            .FirstOrDefaultAsync(cancellationToken);
}
