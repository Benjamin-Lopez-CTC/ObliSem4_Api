using System;

namespace ObliSem4Api.DTO.Filters;

public class TaskFilterDto
{
    // Detalles
    public int? UserId { get; set; }
    public int? ProjectId { get; set; }
    public string? Title { get; set; }      
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string? AccentColor { get; set; }
    public DateTime? CreatedAt { get; set; }

    // Ubicacion (opcional)
    public bool HasLocation { get; set; } = false;

    // Archivo (opcional)
    public bool HasAttachment { get; set; } = false;

    // Subtareas (opcional)
    public bool HasSubtasks { get; set; } = false;

    // Paginacion
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
