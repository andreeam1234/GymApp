using GymApp.Models;

namespace GymApp.DTOs;

public record EnrollmentDto(
    int Id,
    string UserId,
    string UserName,
    int ClassSessionId,
    string GymClassName,
    DateTime SessionStartTime,
    EnrollmentStatus Status,
    DateTime EnrolledAt);
