using GymApp.Models;
using GymApp.Repositories;

namespace GymApp.Services;

public class GymClassService : IGymClassService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GymClassService> _logger;

    public GymClassService(IUnitOfWork unitOfWork, ILogger<GymClassService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<GymClass>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.GymClassRepository.GetAllWithSessionsAsync(cancellationToken);

    public async Task<GymClass?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _unitOfWork.GymClassRepository.GetByIdWithSessionsAsync(id, cancellationToken);

    public async Task<List<GymClass>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
        => await _unitOfWork.GymClassRepository.GetByCategoryAsync(category, cancellationToken);

    public async Task AddAsync(GymClass gymClass, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating gym class {Name}", gymClass.Name);
        await _unitOfWork.GymClassRepository.AddAsync(gymClass, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Gym class created with id {GymClassId}", gymClass.Id);
    }

    public async Task UpdateAsync(GymClass gymClass, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating gym class {GymClassId}", gymClass.Id);
        _unitOfWork.GymClassRepository.Update(gymClass);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var gymClass = await _unitOfWork.GymClassRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"GymClass {id} not found");

        _logger.LogInformation("Deleting gym class {GymClassId}", id);
        _unitOfWork.GymClassRepository.Delete(gymClass);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.GymClassRepository.CountAsync(cancellationToken);

    public async Task<List<GymClass>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _unitOfWork.GymClassRepository.GetPagedAsync(page, pageSize, cancellationToken);
}
