namespace GymApp.Models;

public class WorkoutPlanExercise
{
    public int WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; } = null!;

    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;

    public int Sets { get; set; } = 3;
    public int Reps { get; set; } = 10;
    public int OrderIndex { get; set; }
}
