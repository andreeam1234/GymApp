namespace GymApp.DTOs;

public record WorkoutPlanExerciseDto(
    int ExerciseId,
    string ExerciseName,
    int Sets,
    int Reps,
    int OrderIndex);
