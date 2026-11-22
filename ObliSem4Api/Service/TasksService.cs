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

        public async Task<PagedResult<TaskReadDto>> GetFilteredAsync(int userId, TaskFilterDto filter)
        {
            filter.Title = filter.Title?.Trim();
            filter.Description = filter.Description?.Trim();
            filter.Priority = filter.Priority?.Trim().ToLower();
            filter.AccentColor = filter.AccentColor?.Trim().ToLower();

            //Longitud de textos
            if (filter.Title?.Length > 100)
            {
                throw new ArgumentException("El titulo no puede superar los 100 caracteres.");
            }
            if (filter.Description?.Length > 1000)
            {
                throw new ArgumentException("La descripción no puede superar los 1000 caracteres.");
            }

            //Prioridad valida
            var prioridades = new[] { "low", "high" };
            if (!string.IsNullOrEmpty(filter.Priority) && !prioridades.Contains(filter.Priority.ToLower()))
            {
                throw new ArgumentException("La prioridad no es válida.");
            }

            //Color de acento valido
            var coloresValidos = new[] { "red", "blue", "green", "yellow", "purple", "orange" };
            if (filter.AccentColor != null && !coloresValidos.Contains(filter.AccentColor.ToLower()))
            {
                throw new ArgumentException("El color de acento no es válido.");
            }

            //Fechas coherentes
            if (filter.DueDate.HasValue && filter.CreatedAt.HasValue && filter.DueDate < filter.CreatedAt)
            {
                throw new ArgumentException("La fecha de vencimiento no puede ser anterior a la fecha de creación.");
            }

            if (filter.CompletedAt.HasValue && filter.CreatedAt.HasValue && filter.CompletedAt < filter.CreatedAt)
            {
                throw new ArgumentException("La fecha de finalización no puede ser anterior a la fecha de creación.");
            }

            if(filter.CreatedAt.HasValue && filter.CreatedAt > DateTime.UtcNow)
            {
                throw new ArgumentException("La fecha de creación no puede ser posterior a la fecha actual.");
            }

            //Llamada a repository
            return await _repository.GetFilteredAsync(userId, filter);
        }
    }
}
