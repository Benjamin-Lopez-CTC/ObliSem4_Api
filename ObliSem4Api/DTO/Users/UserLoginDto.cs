using System.ComponentModel.DataAnnotations;

namespace ObliSem4Api.DTO.Users;

public class UserLoginDto
{
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; } = string.Empty;
}
