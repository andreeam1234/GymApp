using System.ComponentModel.DataAnnotations;

namespace GymApp.DTOs;

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password);
