using GymApp.Models;
using GymApp.Repositories;

namespace GymApp.Services;

public class ClassSessionService : IClassSessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClassSessionService> _logger;

    public ClassSessionService(IUnitOfWork unitOfWork, ILogger<ClassSessionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<ClassSession>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.ClassSessionRepository.GetAllWithDetailsAsync(cancellationToken);

    public async Task<ClassSession?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _unitOfWork.ClassSessionRepository.GetByIdWithDetailsAsync(id, cancellationToken);

    public async Task<List<ClassSession>> GetUpcomingAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.ClassSessionRepository.GetUpcomingAsync(cancellationToken);

    public async Task<List<ClassSession>> GetByTrainerAsync(string trainerId, CancellationToken cancellationToken = default)
        => await _unitOfWork.ClassSessionRepository.GetByTrainerAsync(trainerId, cancellationToken);

    public async Task AddAsync(ClassSession session, CancellationToken cancellationToken = default)
    {
        if (session.EndTime <= session.StartTime)
            throw new ArgumentException("End time must be after start time");

        var gymClass = await _unitOfWork.GymClassRepository.GetByIdAsync(session.GymClassId, cancellationToken)
            ?? throw new KeyNotFoundException($"GymClass {session.GymClassId} not found");

        _logger.LogInformation("Creating session for class {GymClassId} at {StartTime}",
            session.GymClassId, session.StartTime);

        await _unitOfWork.ClassSessionRepository.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Session created with id {SessionId}", session.Id);
    }

    public async Task UpdateAsync(ClassSession session, CancellationToken cancellationToken = default)
    {
        if (session.EndTime <= session.StartTime)
            throw new ArgumentException("End time must be after start time");

        _logger.LogInformation("Updating session {SessionId}", session.Id);
        _unitOfWork.ClassSessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var session = await _unitOfWork.ClassSessionRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"ClassSession {id} not found");

        _logger.LogInformation("Deleting session {SessionId}", id);
        _unitOfWork.ClassSessionRepository.Delete(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
