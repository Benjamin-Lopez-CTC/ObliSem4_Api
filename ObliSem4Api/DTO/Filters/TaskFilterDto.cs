using System;

namespace ObliSem4Api.DTO.Filters;

public class TaskFilterDto
{
        // Detalles
        public string? Title { get; set; }        
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string? AccentColor { get; set; }
        public DateTime CreatedAt { get; set; }

        // Ubicacion (opcional)
        public string? ZoneAddress { get; set; }

        // Archivo (opcional)
        public string? AttachmentKey { get; set; }
}
