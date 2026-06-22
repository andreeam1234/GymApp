using GymApp.DTOs;
using GymApp.Models;

namespace GymApp.Mappings;

public static class EnrollmentMappings
{
    public static EnrollmentDto ToDto(this Enrollment enrollment) => new(
        Id: enrollment.Id,
        UserId: enrollment.UserId,
        UserName: enrollment.User?.FullName ?? "N/A",
        ClassSessionId: enrollment.ClassSessionId,
        GymClassName: enrollment.ClassSession?.GymClass?.Name ?? "N/A",
        SessionStartTime: enrollment.ClassSession?.StartTime ?? DateTime.MinValue,
        Status: enrollment.Status,
        EnrolledAt: enrollment.EnrolledAt);

    public static List<EnrollmentDto> ToDtoList(this IEnumerable<Enrollment> enrollments)
        => enrollments.Select(e => e.ToDto()).ToList();
}
