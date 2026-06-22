using System.ComponentModel.DataAnnotations;

namespace GymApp.DTOs;

public record RegisterDto(
    [Required, EmailAddress] string Email,
    [Required, MinLength(2)] string FirstName,
    [Required, MinLength(2)] string LastName,
    [Required, MinLength(6)] string Password);
