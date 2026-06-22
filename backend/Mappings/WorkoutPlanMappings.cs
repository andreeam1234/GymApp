using GymApp.DTOs;
using GymApp.Models;

namespace GymApp.Mappings;

public static class WorkoutPlanMappings
{
    public static WorkoutPlanDto ToDto(this WorkoutPlan plan) => new(
        Id: plan.Id,
        Title: plan.Title,
        Notes: plan.Notes,
        CreatedAt: plan.CreatedAt,
        UserId: plan.UserId,
        UserName: plan.User?.FullName ?? "N/A",
        Exercises: plan.WorkoutPlanExercises?
            .OrderBy(wpe => wpe.OrderIndex)
            .Select(wpe => new WorkoutPlanExerciseDto(
                wpe.ExerciseId,
                wpe.Exercise?.Name ?? "N/A",
                wpe.Sets,
                wpe.Reps,
                wpe.OrderIndex))
            .ToList() ?? new List<WorkoutPlanExerciseDto>());

    public static List<WorkoutPlanDto> ToDtoList(this IEnumerable<WorkoutPlan> plans)
        => plans.Select(p => p.ToDto()).ToList();

    public static WorkoutPlan ToEntity(this CreateWorkoutPlanDto dto, string userId) => new()
    {
        Title = dto.Title,
        Notes = dto.Notes,
        UserId = userId,
        WorkoutPlanExercises = dto.Exercises?
            .Select(e => new WorkoutPlanExercise
            {
                ExerciseId = e.ExerciseId,
                Sets = e.Sets,
                Reps = e.Reps,
                OrderIndex = e.OrderIndex
            }).ToList() ?? new List<WorkoutPlanExercise>()
    };

    public static void ApplyTo(this UpdateWorkoutPlanDto dto, WorkoutPlan entity)
    {
        entity.Title = dto.Title;
        entity.Notes = dto.Notes;
        entity.WorkoutPlanExercises.Clear();
        if (dto.Exercises != null)
        {
            foreach (var e in dto.Exercises)
            {
                entity.WorkoutPlanExercises.Add(new WorkoutPlanExercise
                {
                    ExerciseId = e.ExerciseId,
                    Sets = e.Sets,
                    Reps = e.Reps,
                    OrderIndex = e.OrderIndex
                });
            }
        }
    }
}
