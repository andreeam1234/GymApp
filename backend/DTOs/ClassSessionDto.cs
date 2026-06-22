namespace GymApp.DTOs;

public record ClassSessionDto(
    int Id,
    int GymClassId,
    string GymClassName,
    string TrainerId,
    string TrainerName,
    DateTime StartTime,
    DateTime EndTime,
    int Capacity,
    string Room,
    int EnrolledCount);
