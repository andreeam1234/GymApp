namespace GymApp.Models;

public class GymClass : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string Category { get; set; } = string.Empty; 

    public ICollection<ClassSession> Sessions { get; set; } = new List<ClassSession>();
}
