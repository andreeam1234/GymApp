using GymApp.Models;
using GymApp.Repositories;

namespace GymApp.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EnrollmentService> _logger;

    public EnrollmentService(IUnitOfWork unitOfWork, ILogger<EnrollmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<Enrollment>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
        => await _unitOfWork.EnrollmentRepository.GetByUserAsync(userId, cancellationToken);

    public async Task<List<Enrollment>> GetBySessionAsync(int sessionId, CancellationToken cancellationToken = default)
        => await _unitOfWork.EnrollmentRepository.GetBySessionAsync(sessionId, cancellationToken);

    public async Task<Enrollment> EnrollAsync(string userId, int sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _unitOfWork.ClassSessionRepository.GetByIdAsync(sessionId, cancellationToken)
            ?? throw new KeyNotFoundException($"ClassSession {sessionId} not found");

        var existing = await _unitOfWork.EnrollmentRepository.GetByUserAndSessionAsync(userId, sessionId, cancellationToken);
        if (existing != null && existing.Status == EnrollmentStatus.Confirmed)
            throw new ArgumentException("User is already enrolled in this session");

        var activeCount = await _unitOfWork.EnrollmentRepository.CountActiveBySessionAsync(sessionId, cancellationToken);
        if (activeCount >= session.Capacity)
            throw new InvalidOperationException("Session is fully booked");

        _logger.LogInformation("Enrolling user {UserId} into session {SessionId}", userId, sessionId);

        var enrollment = new Enrollment
        {
            UserId = userId,
            ClassSessionId = sessionId,
            Status = EnrollmentStatus.Confirmed,
            EnrolledAt = DateTime.UtcNow
        };

        await _unitOfWork.EnrollmentRepository.AddAsync(enrollment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Enrollment created with id {EnrollmentId}", enrollment.Id);
        return enrollment;
    }

    public async Task CancelAsync(string userId, int enrollmentId, CancellationToken cancellationToken = default)
    {
        var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(enrollmentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Enrollment {enrollmentId} not found");

        if (enrollment.UserId != userId)
            throw new UnauthorizedAccessException("Cannot cancel another user's enrollment");

        _logger.LogInformation("Cancelling enrollment {EnrollmentId}", enrollmentId);
        enrollment.Status = EnrollmentStatus.Cancelled;
        _unitOfWork.EnrollmentRepository.Update(enrollment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
