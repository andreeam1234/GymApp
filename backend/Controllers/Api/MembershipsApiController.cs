using System.Security.Claims;
using GymApp.DTOs;
using GymApp.Mappings;
using GymApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers.Api;

[ApiController]
[Route("api/memberships")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MembershipsApiController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    public MembershipsApiController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    // GET: /api/memberships (Admin only — all memberships)
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(typeof(List<MembershipDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<MembershipDto>>> GetAll(CancellationToken cancellationToken)
    {
        var memberships = await _membershipService.GetAllAsync(cancellationToken);
        return Ok(memberships.ToDtoList());
    }

    // GET: /api/memberships/mine
    [HttpGet("mine")]
    [ProducesResponseType(typeof(List<MembershipDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<MembershipDto>>> GetMine(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var memberships = await _membershipService.GetByUserAsync(userId, cancellationToken);
        return Ok(memberships.ToDtoList());
    }

    // GET: /api/memberships/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MembershipDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MembershipDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var membership = await _membershipService.GetByIdAsync(id, cancellationToken);
        if (membership == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (membership.UserId != userId && !User.IsInRole("Admin"))
            return Forbid(JwtBearerDefaults.AuthenticationScheme);

        return Ok(membership.ToDto());
    }

    // POST: /api/memberships (Admin only)
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(typeof(MembershipDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<MembershipDto>> Create(CreateMembershipDto dto, CancellationToken cancellationToken)
    {
        var membership = dto.ToEntity();
        await _membershipService.AddAsync(membership, cancellationToken);

        var created = await _membershipService.GetByIdAsync(membership.Id, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = membership.Id }, created!.ToDto());
    }

    // DELETE: /api/memberships/5 (Admin only)
    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var membership = await _membershipService.GetByIdAsync(id, cancellationToken);
        if (membership == null)
            return NotFound();

        await _membershipService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
