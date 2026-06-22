using GymApp.DTOs;
using GymApp.Mappings;
using GymApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers.Api;

[ApiController]
[Route("api/classsessions")]
public class ClassSessionsApiController : ControllerBase
{
    private readonly IClassSessionService _sessionService;

    public ClassSessionsApiController(IClassSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    // GET: /api/classsessions
    [HttpGet]
    [ProducesResponseType(typeof(List<ClassSessionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClassSessionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var sessions = await _sessionService.GetAllAsync(cancellationToken);
        return Ok(sessions.ToDtoList());
    }

    // GET: /api/classsessions/upcoming
    [HttpGet("upcoming")]
    [ProducesResponseType(typeof(List<ClassSessionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClassSessionDto>>> GetUpcoming(CancellationToken cancellationToken)
    {
        var sessions = await _sessionService.GetUpcomingAsync(cancellationToken);
        return Ok(sessions.ToDtoList());
    }

    // GET: /api/classsessions/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClassSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClassSessionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var session = await _sessionService.GetByIdAsync(id, cancellationToken);
        if (session == null)
            return NotFound();

        return Ok(session.ToDto());
    }

    // POST: /api/classsessions (Trainer or Admin)
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Trainer")]
    [ProducesResponseType(typeof(ClassSessionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClassSessionDto>> Create(CreateClassSessionDto dto, CancellationToken cancellationToken)
    {
        var session = dto.ToEntity();
        await _sessionService.AddAsync(session, cancellationToken);

        var created = await _sessionService.GetByIdAsync(session.Id, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = session.Id }, created!.ToDto());
    }

    // PUT: /api/classsessions/5 (Trainer or Admin)
    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Trainer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateClassSessionDto dto, CancellationToken cancellationToken)
    {
        var session = await _sessionService.GetByIdAsync(id, cancellationToken);
        if (session == null)
            return NotFound();

        dto.ApplyTo(session);
        await _sessionService.UpdateAsync(session, cancellationToken);

        return NoContent();
    }

    // DELETE: /api/classsessions/5 (Admin only)
    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var session = await _sessionService.GetByIdAsync(id, cancellationToken);
        if (session == null)
            return NotFound();

        await _sessionService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
