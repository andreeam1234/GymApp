namespace GymApp.ViewModels;

public class ProfileViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<EnrollmentViewModel> MyEnrollments { get; set; } = new();
    public List<WorkoutPlanViewModel> MyWorkoutPlans { get; set; } = new();
    public MembershipViewModel? ActiveMembership { get; set; }
}

public class EnrollmentViewModel
{
    public int Id { get; set; }
    public string GymClassName { get; set; } = string.Empty;
    public DateTime SessionStartTime { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class WorkoutPlanViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ExerciseCount { get; set; }
}

public class MembershipViewModel
{
    public string Type { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}
