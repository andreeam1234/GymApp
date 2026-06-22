using Microsoft.AspNetCore.Identity;

namespace GymApp.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();

    public ICollection<ClassSession> TrainedSessions { get; set; } = new List<ClassSession>();

    public string FullName => $"{FirstName} {LastName}".Trim();
}
