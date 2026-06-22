using System.ComponentModel.DataAnnotations;
using GymApp.Models;

namespace GymApp.DTOs;

public record CreateMembershipDto(
    [Required] string UserId,
    [Required] MembershipType Type,
    [Required, Range(0, 10000)] decimal Price,
    [Required] DateTime StartDate,
    [Required] DateTime EndDate);
