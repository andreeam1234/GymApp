namespace GymApp.DTOs;

public record ExerciseDto(
    int Id,
    string Name,
    string MuscleGroup,
    string Description,
    string Equipment);
