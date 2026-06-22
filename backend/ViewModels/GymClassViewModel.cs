namespace GymApp.ViewModels;

public class GymClassViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string Category { get; set; } = string.Empty;
    public int SessionCount { get; set; }
    public List<ClassSessionViewModel> Sessions { get; set; } = new();
}

public class ClassSessionViewModel
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
    public int EnrolledCount { get; set; }
    public string Room { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
}
