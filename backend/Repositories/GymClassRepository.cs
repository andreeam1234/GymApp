using GymApp.Data;
using GymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repositories;

public class GymClassRepository : Repository<GymClass>, IGymClassRepository
{
    public GymClassRepository(AppDbContext context) : base(context) { }

    public async Task<List<GymClass>> GetAllWithSessionsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.GymClasses
            .Include(gc => gc.Sessions)
            .OrderBy(gc => gc.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<GymClass?> GetByIdWithSessionsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.GymClasses
            .Include(gc => gc.Sessions)
                .ThenInclude(s => s.Trainer)
            .FirstOrDefaultAsync(gc => gc.Id == id, cancellationToken);
    }

    public async Task<List<GymClass>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return await _context.GymClasses
            .Where(gc => gc.Category == category)
            .Include(gc => gc.Sessions)
            .OrderBy(gc => gc.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        => await _context.GymClasses.CountAsync(cancellationToken);

    public async Task<List<GymClass>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.GymClasses
            .Include(gc => gc.Sessions)
            .OrderBy(gc => gc.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
