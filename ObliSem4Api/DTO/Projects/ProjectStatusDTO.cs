using System;
using System.ComponentModel.DataAnnotations;

namespace ObliSem4Api.DTO;

public class ProjectStatusDTO
{
    [Required]
    [RegularExpression("^(Pendiente|En progreso|Completado)$", ErrorMessage = "El estado debe ser 'Pendiente', 'En progreso' o 'Completado'.")]
    public string Status { get; set; } = string.Empty;
}
