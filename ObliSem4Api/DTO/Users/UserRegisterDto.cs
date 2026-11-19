using System.ComponentModel.DataAnnotations;

namespace ObliSem4Api.DTO.Users;

public class UserRegisterDto
{
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es valido.")]
    [StringLength(255, ErrorMessage = "El email no puede superar los 255 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(50, MinimumLength = 8, 
        ErrorMessage = "La contraseña debe tener entre 8 y 50 caracteres.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$",
        ErrorMessage = "La contraseña debe incluir letras y números.")]
    public string Password { get; set; } = string.Empty;
}
