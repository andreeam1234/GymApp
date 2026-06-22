using GymApp.Models;
using GymApp.Repositories;

namespace GymApp.Services;

public class MembershipService : IMembershipService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MembershipService> _logger;

    public MembershipService(IUnitOfWork unitOfWork, ILogger<MembershipService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<Membership>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.MembershipRepository.GetAllAsync(cancellationToken);

    public async Task<Membership?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _unitOfWork.MembershipRepository.GetByIdAsync(id, cancellationToken);

    public async Task<List<Membership>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
        => await _unitOfWork.MembershipRepository.GetByUserAsync(userId, cancellationToken);

    public async Task<Membership?> GetActiveByUserAsync(string userId, CancellationToken cancellationToken = default)
        => await _unitOfWork.MembershipRepository.GetActiveByUserAsync(userId, cancellationToken);

    public async Task AddAsync(Membership membership, CancellationToken cancellationToken = default)
    {
        if (membership.EndDate <= membership.StartDate)
            throw new ArgumentException("End date must be after start date");

        _logger.LogInformation("Creating membership for user {UserId}", membership.UserId);
        await _unitOfWork.MembershipRepository.AddAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var membership = await _unitOfWork.MembershipRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Membership {id} not found");

        _logger.LogInformation("Deleting membership {MembershipId}", id);
        _unitOfWork.MembershipRepository.Delete(membership);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
