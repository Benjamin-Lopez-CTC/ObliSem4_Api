using System;
using System.ComponentModel.DataAnnotations;

namespace ObliSem4Api.DTO.Tasks;

public class TaskCreateDto
{
        [Required(ErrorMessage = "El titulo es obligatorio.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede tener mas de 500 caracteres.")]
        public string? Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        [RegularExpression("^low$|^high$", ErrorMessage = "Prioridad invalida.")]
        public string Priority { get; set; } = string.Empty;
        public int? ProjectId { get; set; }
        public DateTime? DueDate { get; set; }
        
        [RegularExpression("^red$|^blue$|^green$|^yellow$|^purple$|^orange$", ErrorMessage = "Color invalido.")]
        public string? AccentColor { get; set; }        
        
        //Ubicacion
        public double? ZoneLatitude { get; set; }
        public double? ZoneLongitude { get; set; }
        public int? ZoneRadiusMeters { get; set; }
        public string? ZoneAddress { get; set; }
        
        //Archivos
        public string? AttachmentKey { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentType { get; set; }
        public string? AttachmentUrl { get; set; }
}
