namespace GymApp.Models;

public class Exercise : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string MuscleGroup { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;

    public ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
}
