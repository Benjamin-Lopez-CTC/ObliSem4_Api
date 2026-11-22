using System;
using System.ComponentModel.DataAnnotations;

namespace ObliSem4Api.DTO.Projects;

public class ProjectStatusDTO
{
    [Required]
    [AllowedValues("Pendiente", "En progreso", "Completado", "Archivado", ErrorMessage = "El estado debe ser 'Pendiente', 'En progreso', 'Completado' o 'Archivado'.")]
    public string Status { get; set; } = string.Empty;
}
