using GymApp.DTOs;
using GymApp.Models;

namespace GymApp.Mappings;

public static class ClassSessionMappings
{
    public static ClassSessionDto ToDto(this ClassSession session) => new(
        Id: session.Id,
        GymClassId: session.GymClassId,
        GymClassName: session.GymClass?.Name ?? "N/A",
        TrainerId: session.TrainerId,
        TrainerName: session.Trainer?.FullName ?? "N/A",
        StartTime: session.StartTime,
        EndTime: session.EndTime,
        Capacity: session.Capacity,
        Room: session.Room,
        EnrolledCount: session.Enrollments?.Count(e => e.Status == EnrollmentStatus.Confirmed) ?? 0);

    public static List<ClassSessionDto> ToDtoList(this IEnumerable<ClassSession> sessions)
        => sessions.Select(s => s.ToDto()).ToList();

    public static ClassSession ToEntity(this CreateClassSessionDto dto) => new()
    {
        GymClassId = dto.GymClassId,
        TrainerId = dto.TrainerId,
        StartTime = dto.StartTime,
        EndTime = dto.EndTime,
        Capacity = dto.Capacity,
        Room = dto.Room
    };

    public static void ApplyTo(this UpdateClassSessionDto dto, ClassSession entity)
    {
        entity.StartTime = dto.StartTime;
        entity.EndTime = dto.EndTime;
        entity.Capacity = dto.Capacity;
        entity.Room = dto.Room;
    }
}
