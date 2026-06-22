using GymApp.DTOs;
using GymApp.Models;

namespace GymApp.Mappings;

public static class MembershipMappings
{
    public static MembershipDto ToDto(this Membership membership) => new(
        Id: membership.Id,
        Type: membership.Type,
        Price: membership.Price,
        StartDate: membership.StartDate,
        EndDate: membership.EndDate,
        IsActive: membership.IsActive,
        UserId: membership.UserId,
        UserName: membership.User?.FullName ?? "N/A");

    public static List<MembershipDto> ToDtoList(this IEnumerable<Membership> memberships)
        => memberships.Select(m => m.ToDto()).ToList();

    public static Membership ToEntity(this CreateMembershipDto dto) => new()
    {
        UserId = dto.UserId,
        Type = dto.Type,
        Price = dto.Price,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        IsActive = true
    };
}
