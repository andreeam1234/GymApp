using System.Security.Claims;
using GymApp.DTOs;
using GymApp.Mappings;
using GymApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers.Api;

[ApiController]
[Route("api/enrollments")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EnrollmentsApiController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsApiController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    // GET: /api/enrollments (mine)
    [HttpGet]
    [ProducesResponseType(typeof(List<EnrollmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<EnrollmentDto>>> GetMine(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var enrollments = await _enrollmentService.GetByUserAsync(userId, cancellationToken);
        return Ok(enrollments.ToDtoList());
    }

    // GET: /api/enrollments/session/5 (Trainer/Admin — roster for a session)
    [HttpGet("session/{sessionId:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Trainer")]
    [ProducesResponseType(typeof(List<EnrollmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<EnrollmentDto>>> GetBySession(int sessionId, CancellationToken cancellationToken)
    {
        var enrollments = await _enrollmentService.GetBySessionAsync(sessionId, cancellationToken);
        return Ok(enrollments.ToDtoList());
    }

    // POST: /api/enrollments (book a session)
    [HttpPost]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EnrollmentDto>> Create(CreateEnrollmentDto dto, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var enrollment = await _enrollmentService.EnrollAsync(userId, dto.ClassSessionId, cancellationToken);

        var dtoResult = (await _enrollmentService.GetByUserAsync(userId, cancellationToken))
            .First(e => e.Id == enrollment.Id);

        return CreatedAtAction(nameof(GetMine), null, dtoResult.ToDto());
    }

    // DELETE: /api/enrollments/5 (cancel my booking)
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _enrollmentService.CancelAsync(userId, id, cancellationToken);
        return NoContent();
    }
}
