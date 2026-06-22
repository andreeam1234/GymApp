using GymApp.DTOs;
using GymApp.Models;

namespace GymApp.Mappings;

public static class ExerciseMappings
{
    public static ExerciseDto ToDto(this Exercise exercise) => new(
        Id: exercise.Id,
        Name: exercise.Name,
        MuscleGroup: exercise.MuscleGroup,
        Description: exercise.Description,
        Equipment: exercise.Equipment);

    public static List<ExerciseDto> ToDtoList(this IEnumerable<Exercise> exercises)
        => exercises.Select(e => e.ToDto()).ToList();

    public static Exercise ToEntity(this CreateExerciseDto dto) => new()
    {
        Name = dto.Name,
        MuscleGroup = dto.MuscleGroup,
        Description = dto.Description,
        Equipment = dto.Equipment
    };

    public static void ApplyTo(this UpdateExerciseDto dto, Exercise entity)
    {
        entity.Name = dto.Name;
        entity.MuscleGroup = dto.MuscleGroup;
        entity.Description = dto.Description;
        entity.Equipment = dto.Equipment;
    }
}
