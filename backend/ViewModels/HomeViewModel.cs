namespace GymApp.ViewModels;

public class HomeViewModel
{
    public List<GymClassViewModel> FeaturedClasses { get; set; } = new();
    public int TotalClasses { get; set; }
    public int TotalExercises { get; set; }
}
