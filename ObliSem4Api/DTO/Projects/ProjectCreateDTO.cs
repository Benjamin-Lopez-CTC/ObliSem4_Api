using System;
using System.ComponentModel.DataAnnotations;

namespace ObliSem4Api.DTO;

public class ProjectCreateDTO
{   
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede tener mas de 500 caracteres.")]
    public string? Description { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(Pendiente|En progreso|Completado)$", ErrorMessage = "El estado debe ser 'Pendiente', 'En progreso' o 'Completado'.")]
    public string Status { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    [RegularExpression("^red$|^blue$|^green$|^yellow$|^purple$|^orange$", ErrorMessage = "Color invalido.")]
    public string? AccentColor { get; set; }
}
