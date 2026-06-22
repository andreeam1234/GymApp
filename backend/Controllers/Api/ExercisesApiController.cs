using GymApp.DTOs;
using GymApp.Mappings;
using GymApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers.Api;

[ApiController]
[Route("api/exercises")]
public class ExercisesApiController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExercisesApiController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    // GET: /api/exercises
    [HttpGet]
    [ProducesResponseType(typeof(List<ExerciseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ExerciseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var exercises = await _exerciseService.GetAllAsync(cancellationToken);
        return Ok(exercises.ToDtoList());
    }

    // GET: /api/exercises/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ExerciseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExerciseDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var exercise = await _exerciseService.GetByIdAsync(id, cancellationToken);
        if (exercise == null)
            return NotFound();

        return Ok(exercise.ToDto());
    }

    // POST: /api/exercises (Admin only)
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(typeof(ExerciseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ExerciseDto>> Create(CreateExerciseDto dto, CancellationToken cancellationToken)
    {
        var exercise = dto.ToEntity();
        await _exerciseService.AddAsync(exercise, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = exercise.Id }, exercise.ToDto());
    }

    // PUT: /api/exercises/5 (Admin only)
    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateExerciseDto dto, CancellationToken cancellationToken)
    {
        var exercise = await _exerciseService.GetByIdAsync(id, cancellationToken);
        if (exercise == null)
            return NotFound();

        dto.ApplyTo(exercise);
        await _exerciseService.UpdateAsync(exercise, cancellationToken);

        return NoContent();
    }

    // DELETE: /api/exercises/5 (Admin only)
    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var exercise = await _exerciseService.GetByIdAsync(id, cancellationToken);
        if (exercise == null)
            return NotFound();

        await _exerciseService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
