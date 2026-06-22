namespace GymApp.Models;

public class ClassSession : BaseEntity
{
    public int GymClassId { get; set; }
    public GymClass GymClass { get; set; } = null!;

    public string TrainerId { get; set; } = string.Empty;
    public ApplicationUser Trainer { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
    public string Room { get; set; } = string.Empty;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
