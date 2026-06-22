using System.ComponentModel.DataAnnotations;

namespace GymApp.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Prenumele este obligatoriu")]
    [MinLength(2, ErrorMessage = "Prenumele trebuie sa aiba minim 2 caractere")]
    [Display(Name = "Prenume")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Numele este obligatoriu")]
    [MinLength(2, ErrorMessage = "Numele trebuie sa aiba minim 2 caractere")]
    [Display(Name = "Nume")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email-ul este obligatoriu")]
    [EmailAddress(ErrorMessage = "Format email invalid")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola este obligatorie")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Parola trebuie sa aiba minim 6 caractere")]
    [Display(Name = "Parola")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmarea parolei este obligatorie")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Parolele nu coincid")]
    [Display(Name = "Confirmare parola")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
