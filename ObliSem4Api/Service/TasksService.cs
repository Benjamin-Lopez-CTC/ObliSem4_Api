using Microsoft.AspNetCore.Mvc;
using ObliSem4Api.DTO.Filters;
using ObliSem4Api.DTO.Tasks;
using ObliSem4Api.Interfaces;
using ObliSem4Api.Models;

namespace ObliSem4Api.Service
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _repository;

        public TasksService(ITasksRepository repository)
        {
            _repository = repository;
        }

        // Convierte el objeto tarea al dto de lectura para omitir informacion innecesaria

        private static TaskReadDto ToReadDto(Tasks task)
        {
            return new TaskReadDto
            {
                // Detalles
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CompletedAt = task.CompletedAt,
                IsDeleted = task.IsDeleted,
                DueEmailSent = task.DueEmailSent,
                AccentColor = task.AccentColor,
                CreatedAt = task.CreatedAt,

                // Proyecto
                IdProject = task.ProjectId,

                // Ubicación
                ZoneLatitude = task.ZoneLatitude,
                ZoneLongitude = task.ZoneLongitude,
                ZoneRadiusMeters = task.ZoneRadiusMeters,
                ZoneAddress = task.ZoneAddress,

                // Archivo
                AttachmentKey = task.AttachmentKey,
                AttachmentName = task.AttachmentName,
                AttachmentType = task.AttachmentType,
                AttachmentUrl = task.AttachmentUrl
            };
        }

        public async Task<PagedResult<TaskReadDto>> GetAll(
            string? Title,
            string? Description,
            string? Priority,
            DateTime? DueDate,
            DateTime? CompletedAt,
            bool? IsDeleted,
            string? AccentColor,
            DateTime? CreatedAt,
            bool? HasLocation,
            bool? HasAttachment,
            int page = 1,
            int pageSize = 10
            )
        {
            //Longitud de textos
            if (Title?.Length > 100)
            {
                throw new ArgumentException("El titulo no puede superar los 100 caracteres.");
            }
            if (Description?.Length > 1000)
            {
                throw new ArgumentException("La descripción no puede superar los 1000 caracteres.");
            }

            //Prioridad valida
            var prioridades = new[] { "low", "high" };
            if (Priority != null && !prioridades.Contains(Priority.ToLower()))
            {
                throw new ArgumentException("La prioridad no es válida.");
            }

            //Color de acento valido
            var coloresValidos = new[] { "red", "`blue", "green", "yellow", "purple", "orange" };
            if (AccentColor != null && !coloresValidos.Contains(AccentColor.ToLower()))
            {
                throw new ArgumentException("El color de acento no es válido.");
            }

            //Fechas coherentes
            if (DueDate.HasValue && CreatedAt.HasValue && DueDate < CreatedAt)
            {
                throw new ArgumentException("La fecha de vencimiento no puede ser anterior a la fecha de creación.");
            }

            if (CompletedAt.HasValue && CreatedAt.HasValue && CompletedAt < CreatedAt)
            {
                throw new ArgumentException("La fecha de finalización no puede ser anterior a la fecha de creación.");
            }

            if(CompletedAt.HasValue && DueDate.HasValue && CompletedAt < DueDate)
            {
                throw new ArgumentException("La fecha de finalización no puede ser anterior a la fecha de vencimiento.");
            }

            if(CreatedAt.HasValue && CreatedAt > DateTime.Now)
            {
                throw new ArgumentException("La fecha de creación no puede ser posterior a la fecha actual.");
            }

            //Llamada a repository

                return await _repository.GetAll(
                Title?.Trim(),
                Description?.Trim(),
                Priority?.Trim(),
                DueDate,
                CompletedAt,
                IsDeleted,
                AccentColor?.Trim(),
                CreatedAt,
                HasLocation,
                HasAttachment,
                page,
                pageSize
            );
        }
    }
}
