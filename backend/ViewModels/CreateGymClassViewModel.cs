using System.ComponentModel.DataAnnotations;

namespace GymApp.ViewModels;

public class CreateGymClassViewModel
{
    [Required(ErrorMessage = "Numele este obligatoriu")]
    [MinLength(3, ErrorMessage = "Numele trebuie sa aiba minim 3 caractere")]
    [Display(Name = "Nume clasa")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrierea este obligatorie")]
    [MinLength(10, ErrorMessage = "Descrierea trebuie sa aiba minim 10 caractere")]
    [Display(Name = "Descriere")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Durata este obligatorie")]
    [Range(10, 240, ErrorMessage = "Durata trebuie sa fie intre 10 si 240 de minute")]
    [Display(Name = "Durata (minute)")]
    public int DurationMinutes { get; set; }

    [Required(ErrorMessage = "Categoria este obligatorie")]
    [Display(Name = "Categorie")]
    public string Category { get; set; } = string.Empty;
}
