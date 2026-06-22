using GymApp.Data;

namespace GymApp.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private IGymClassRepository? _gymClassRepository;
    private IClassSessionRepository? _classSessionRepository;
    private IExerciseRepository? _exerciseRepository;
    private IWorkoutPlanRepository? _workoutPlanRepository;
    private IMembershipRepository? _membershipRepository;
    private IEnrollmentRepository? _enrollmentRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IGymClassRepository GymClassRepository
        => _gymClassRepository ??= new GymClassRepository(_context);

    public IClassSessionRepository ClassSessionRepository
        => _classSessionRepository ??= new ClassSessionRepository(_context);

    public IExerciseRepository ExerciseRepository
        => _exerciseRepository ??= new ExerciseRepository(_context);

    public IWorkoutPlanRepository WorkoutPlanRepository
        => _workoutPlanRepository ??= new WorkoutPlanRepository(_context);

    public IMembershipRepository MembershipRepository
        => _membershipRepository ??= new MembershipRepository(_context);

    public IEnrollmentRepository EnrollmentRepository
        => _enrollmentRepository ??= new EnrollmentRepository(_context);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
