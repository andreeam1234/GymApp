namespace GymApp.Models;

public class Enrollment : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int ClassSessionId { get; set; }
    public ClassSession ClassSession { get; set; } = null!;

    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Confirmed;
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
}
