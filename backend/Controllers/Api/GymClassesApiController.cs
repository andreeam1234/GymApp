using GymApp.DTOs;
using GymApp.Mappings;
using GymApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers.Api;

[ApiController]
[Route("api/gymclasses")]
public class GymClassesApiController : ControllerBase
{
    private readonly IGymClassService _gymClassService;

    public GymClassesApiController(IGymClassService gymClassService)
    {
        _gymClassService = gymClassService;
    }

    // GET: /api/gymclasses
    [HttpGet]
    [ProducesResponseType(typeof(List<GymClassDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GymClassDto>>> GetAll(CancellationToken cancellationToken)
    {
        var classes = await _gymClassService.GetAllAsync(cancellationToken);
        return Ok(classes.ToDtoList());
    }

    // GET: /api/gymclasses/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GymClassDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GymClassDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var gymClass = await _gymClassService.GetByIdAsync(id, cancellationToken);
        if (gymClass == null)
            return NotFound();

        return Ok(gymClass.ToDto());
    }

    // POST: /api/gymclasses (Admin only)
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(typeof(GymClassDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GymClassDto>> Create(CreateGymClassDto dto, CancellationToken cancellationToken)
    {
        var gymClass = dto.ToEntity();
        await _gymClassService.AddAsync(gymClass, cancellationToken);

        var created = await _gymClassService.GetByIdAsync(gymClass.Id, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = gymClass.Id }, created!.ToDto());
    }

    // PUT: /api/gymclasses/5 (Admin only)
    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateGymClassDto dto, CancellationToken cancellationToken)
    {
        var gymClass = await _gymClassService.GetByIdAsync(id, cancellationToken);
        if (gymClass == null)
            return NotFound();

        dto.ApplyTo(gymClass);
        await _gymClassService.UpdateAsync(gymClass, cancellationToken);

        return NoContent();
    }

    // DELETE: /api/gymclasses/5 (Admin only)
    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var gymClass = await _gymClassService.GetByIdAsync(id, cancellationToken);
        if (gymClass == null)
            return NotFound();

        await _gymClassService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
