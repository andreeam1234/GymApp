using System.Security.Claims;
using GymApp.Models;
using GymApp.Services;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEnrollmentService _enrollmentService;
    private readonly IWorkoutPlanService _workoutPlanService;
    private readonly IMembershipService _membershipService;

    public ProfileController(
        UserManager<ApplicationUser> userManager,
        IEnrollmentService enrollmentService,
        IWorkoutPlanService workoutPlanService,
        IMembershipService membershipService)
    {
        _userManager = userManager;
        _enrollmentService = enrollmentService;
        _workoutPlanService = workoutPlanService;
        _membershipService = membershipService;
    }

    // GET: /Profile
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var enrollments = await _enrollmentService.GetByUserAsync(userId, cancellationToken);
        var plans = await _workoutPlanService.GetByUserAsync(userId, cancellationToken);
        var activeMembership = await _membershipService.GetActiveByUserAsync(userId, cancellationToken);

        var viewModel = new ProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email!,
            Roles = roles.ToList(),
            MyEnrollments = enrollments.Select(e => new EnrollmentViewModel
            {
                Id = e.Id,
                GymClassName = e.ClassSession.GymClass.Name,
                SessionStartTime = e.ClassSession.StartTime,
                Status = e.Status.ToString()
            }).ToList(),
            MyWorkoutPlans = plans.Select(p => new WorkoutPlanViewModel
            {
                Id = p.Id,
                Title = p.Title,
                CreatedAt = p.CreatedAt,
                ExerciseCount = p.WorkoutPlanExercises.Count
            }).ToList(),
            ActiveMembership = activeMembership == null ? null : new MembershipViewModel
            {
                Type = activeMembership.Type.ToString(),
                EndDate = activeMembership.EndDate,
                IsActive = activeMembership.IsActive
            }
        };

        return View(viewModel);
    }
}
