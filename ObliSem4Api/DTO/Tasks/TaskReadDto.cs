using System;

namespace ObliSem4Api.DTO.Tasks;

public class TaskReadDto
{
        // Detalles
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool DueEmailSent { get; set; }
        public string? AccentColor { get; set; }
        public DateTime CreatedAt { get; set; }

        // Proyectos (opcional)
        public int? IdProject { get; set; }

        // Ubicacion (opcional)
        public double? ZoneLatitude { get; set; }
        public double? ZoneLongitude { get; set; }
        public int? ZoneRadiusMeters { get; set; }
        public string? ZoneAddress { get; set; }

        // Archivo (opcional)
        public string? AttachmentKey { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentType { get; set; }
        public string? AttachmentUrl { get; set; }
}
