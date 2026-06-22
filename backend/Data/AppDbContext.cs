namespace GymApp.Data;

using GymApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<GymClass> GymClasses { get; set; }
    public DbSet<ClassSession> ClassSessions { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
    public DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }
    public DbSet<Membership> Memberships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // One-to-Many: GymClass -> ClassSession
        modelBuilder.Entity<ClassSession>()
            .HasOne(cs => cs.GymClass)
            .WithMany(gc => gc.Sessions)
            .HasForeignKey(cs => cs.GymClassId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-One: ClassSession -> Trainer (ApplicationUser)
        modelBuilder.Entity<ClassSession>()
            .HasOne(cs => cs.Trainer)
            .WithMany(u => u.TrainedSessions)
            .HasForeignKey(cs => cs.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-Many: ApplicationUser -> Membership
        modelBuilder.Entity<Membership>()
            .HasOne(m => m.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-Many: ApplicationUser <-> ClassSession, via explicit junction Enrollment
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.User)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.ClassSession)
            .WithMany(cs => cs.Enrollments)
            .HasForeignKey(e => e.ClassSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.UserId, e.ClassSessionId })
            .IsUnique();

        // One-to-Many: ApplicationUser -> WorkoutPlan
        modelBuilder.Entity<WorkoutPlan>()
            .HasOne(wp => wp.User)
            .WithMany(u => u.WorkoutPlans)
            .HasForeignKey(wp => wp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-Many: WorkoutPlan <-> Exercise, via explicit junction WorkoutPlanExercise
        modelBuilder.Entity<WorkoutPlanExercise>()
            .HasKey(wpe => new { wpe.WorkoutPlanId, wpe.ExerciseId });

        modelBuilder.Entity<WorkoutPlanExercise>()
            .HasOne(wpe => wpe.WorkoutPlan)
            .WithMany(wp => wp.WorkoutPlanExercises)
            .HasForeignKey(wpe => wpe.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkoutPlanExercise>()
            .HasOne(wpe => wpe.Exercise)
            .WithMany(e => e.WorkoutPlanExercises)
            .HasForeignKey(wpe => wpe.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Membership>()
            .Property(m => m.Price)
            .HasColumnType("decimal(18,2)");
    }
}
