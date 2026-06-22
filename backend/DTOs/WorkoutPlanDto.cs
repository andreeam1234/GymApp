namespace GymApp.DTOs;

public record WorkoutPlanDto(
    int Id,
    string Title,
    string Notes,
    DateTime CreatedAt,
    string UserId,
    string UserName,
    List<WorkoutPlanExerciseDto> Exercises);
