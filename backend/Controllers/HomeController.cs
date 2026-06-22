using GymApp.Mappings;
using GymApp.Services;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers;

public class HomeController : Controller
{
    private readonly IGymClassService _gymClassService;
    private readonly IExerciseService _exerciseService;

    public HomeController(IGymClassService gymClassService, IExerciseService exerciseService)
    {
        _gymClassService = gymClassService;
        _exerciseService = exerciseService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var featured = await _gymClassService.GetPagedAsync(1, 3, cancellationToken);
        var totalClasses = await _gymClassService.CountAsync(cancellationToken);
        var exercises = await _exerciseService.GetAllAsync(cancellationToken);

        var viewModel = new HomeViewModel
        {
            FeaturedClasses = featured.ToViewModelList(),
            TotalClasses = totalClasses,
            TotalExercises = exercises.Count
        };

        return View(viewModel);
    }
}
