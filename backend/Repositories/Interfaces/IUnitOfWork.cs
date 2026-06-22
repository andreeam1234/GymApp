namespace GymApp.Repositories;

public interface IUnitOfWork
{
    IGymClassRepository GymClassRepository { get; }
    IClassSessionRepository ClassSessionRepository { get; }
    IExerciseRepository ExerciseRepository { get; }
    IWorkoutPlanRepository WorkoutPlanRepository { get; }
    IMembershipRepository MembershipRepository { get; }
    IEnrollmentRepository EnrollmentRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
