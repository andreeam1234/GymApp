using GymApp.DTOs;
using GymApp.Models;
using GymApp.ViewModels;

namespace GymApp.Mappings;

public static class GymClassMappings
{
    public static GymClassViewModel ToViewModel(this GymClass gymClass) => new()
    {
        Id = gymClass.Id,
        Name = gymClass.Name,
        Description = gymClass.Description,
        DurationMinutes = gymClass.DurationMinutes,
        Category = gymClass.Category,
        SessionCount = gymClass.Sessions?.Count ?? 0,
        Sessions = gymClass.Sessions?.Select(s => new ClassSessionViewModel
        {
            Id = s.Id,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            Capacity = s.Capacity,
            EnrolledCount = s.Enrollments?.Count(e => e.Status == EnrollmentStatus.Confirmed) ?? 0,
            Room = s.Room,
            TrainerName = s.Trainer?.FullName ?? "N/A"
        }).ToList() ?? new List<ClassSessionViewModel>()
    };

    public static List<GymClassViewModel> ToViewModelList(this IEnumerable<GymClass> classes)
        => classes.Select(c => c.ToViewModel()).ToList();

    public static GymClassDto ToDto(this GymClass gymClass) => new(
        Id: gymClass.Id,
        Name: gymClass.Name,
        Description: gymClass.Description,
        DurationMinutes: gymClass.DurationMinutes,
        Category: gymClass.Category,
        SessionCount: gymClass.Sessions?.Count ?? 0);

    public static List<GymClassDto> ToDtoList(this IEnumerable<GymClass> classes)
        => classes.Select(c => c.ToDto()).ToList();

    public static GymClass ToEntity(this CreateGymClassDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        DurationMinutes = dto.DurationMinutes,
        Category = dto.Category
    };

    public static void ApplyTo(this UpdateGymClassDto dto, GymClass entity)
    {
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.DurationMinutes = dto.DurationMinutes;
        entity.Category = dto.Category;
    }
}
