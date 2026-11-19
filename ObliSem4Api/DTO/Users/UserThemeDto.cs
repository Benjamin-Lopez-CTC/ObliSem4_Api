using System.ComponentModel.DataAnnotations;

namespace ObliSem4Api.DTO.Users;

public class UserThemeDto
{
    [AllowedValues("light", "dark", null, ErrorMessage = "El tema solo puede ser 'light', 'dark' o nulo.")]
    public string? Theme { get; set; }
}
