using System.Security.Claims;
using GymApp.DTOs;
using GymApp.Mappings;
using GymApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers.Api;

[ApiController]
[Route("api/workoutplans")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WorkoutPlansApiController : ControllerBase
{
    private readonly IWorkoutPlanService _planService;

    public WorkoutPlansApiController(IWorkoutPlanService planService)
    {
        _planService = planService;
    }

    // GET: /api/workoutplans (mine)
    [HttpGet]
    [ProducesResponseType(typeof(List<WorkoutPlanDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<WorkoutPlanDto>>> GetMine(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var plans = await _planService.GetByUserAsync(userId, cancellationToken);
        return Ok(plans.ToDtoList());
    }

    // GET: /api/workoutplans/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WorkoutPlanDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkoutPlanDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var plan = await _planService.GetByIdAsync(id, cancellationToken);
        if (plan == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (plan.UserId != userId && !User.IsInRole("Admin"))
            return Forbid(JwtBearerDefaults.AuthenticationScheme);

        return Ok(plan.ToDto());
    }

    // POST: /api/workoutplans
    [HttpPost]
    [ProducesResponseType(typeof(WorkoutPlanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<WorkoutPlanDto>> Create(CreateWorkoutPlanDto dto, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var plan = dto.ToEntity(userId);
        await _planService.AddAsync(plan, cancellationToken);

        var created = await _planService.GetByIdAsync(plan.Id, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = plan.Id }, created!.ToDto());
    }

    // PUT: /api/workoutplans/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateWorkoutPlanDto dto, CancellationToken cancellationToken)
    {
        var plan = await _planService.GetByIdAsync(id, cancellationToken);
        if (plan == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (plan.UserId != userId && !User.IsInRole("Admin"))
            return Forbid(JwtBearerDefaults.AuthenticationScheme);

        dto.ApplyTo(plan);
        await _planService.UpdateAsync(plan, cancellationToken);

        return NoContent();
    }

    // DELETE: /api/workoutplans/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var plan = await _planService.GetByIdAsync(id, cancellationToken);
        if (plan == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (plan.UserId != userId && !User.IsInRole("Admin"))
            return Forbid(JwtBearerDefaults.AuthenticationScheme);

        await _planService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
