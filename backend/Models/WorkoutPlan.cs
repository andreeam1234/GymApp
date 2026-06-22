namespace GymApp.Models;

public class WorkoutPlan : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
}
