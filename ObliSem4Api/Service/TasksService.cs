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

        // Convierte el objeto tarea al dto de lectura para omitir informacion innecesaria.
        private static TaskReadDto ToReadDto(Tasks task)
        {
            return new TaskReadDto
            {
                // Detalles
                Id = task.Id,
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

        // Convierte el dto de creacion de tarea al modelo de tarea.
        private static Tasks ToModel(TaskCreateDto taskCreateDto, int userId)
        {
            return new Tasks
            {
                UserId = userId,
                Title = taskCreateDto.Title,
                Description = taskCreateDto.Description,
                Priority = taskCreateDto.Priority,
                DueDate = taskCreateDto.DueDate,
                CompletedAt = null,
                IsDeleted = false,
                DueEmailSent = false,
                AccentColor = taskCreateDto.AccentColor,
                CreatedAt = DateTime.UtcNow,
                ProjectId = taskCreateDto.ProjectId,
                ZoneLatitude = taskCreateDto.ZoneLatitude,
                ZoneLongitude = taskCreateDto.ZoneLongitude,
                ZoneRadiusMeters = taskCreateDto.ZoneRadiusMeters,
                ZoneAddress = taskCreateDto.ZoneAddress,
                AttachmentKey = taskCreateDto.AttachmentKey,
                AttachmentName = taskCreateDto.AttachmentName,
                AttachmentType = taskCreateDto.AttachmentType,
                AttachmentUrl = taskCreateDto.AttachmentUrl
            };
        }

        // Obtiene todas las tareas del usuario actualmente en sesion, con filtros opcionales.
        public async Task<PagedResult<TaskReadDto>> GetFilteredAsync(int userId, TaskFilterDto filter)
        {
            filter.Title = filter.Title?.Trim();
            filter.Description = filter.Description?.Trim();
            filter.Priority = filter.Priority?.Trim().ToLower();
            filter.AccentColor = filter.AccentColor?.Trim().ToLower();

            ValidarDatos(
                filter.Title,
                filter.Description,
                filter.Priority,
                filter.DueDate,
                filter.CompletedAt,
                filter.AccentColor,
                filter.CreatedAt
            );

            return await _repository.GetFilteredAsync(userId, filter);
        }

        // Crea una nueva tarea para el usuario actualmente en sesion.
        public async Task<TaskReadDto> CreateAsync(int userId, TaskCreateDto taskCreateDto)
        {
            taskCreateDto.Title = taskCreateDto.Title?.Trim();
            taskCreateDto.Description = taskCreateDto.Description?.Trim();

            if (string.IsNullOrWhiteSpace(taskCreateDto.Title))
            {
                throw new ArgumentException("El titulo es obligatorio.");
            }

            if (taskCreateDto.DueDate.HasValue && taskCreateDto.DueDate.Value.Date < DateTime.UtcNow.Date.AddDays(-1))
            {
                throw new ArgumentException("La fecha de vencimiento no puede estar en el pasado.");
            }

            ValidarDatos(
                taskCreateDto.Title,
                taskCreateDto.Description,
                taskCreateDto.Priority,
                taskCreateDto.DueDate,
                null, //CompletedAt
                taskCreateDto.AccentColor,
                null //CreatedAt
            );

            //Creacion de la tarea            
            var tarea = ToModel(taskCreateDto, userId);
            await _repository.CreateAsync(tarea);
            return ToReadDto(tarea);
        }

        //Metodo para validaciones.
        private static void ValidarDatos(
            string? Title,
            string? Description,
            string? Priority,
            DateTime? DueDate,
            DateTime? CompletedAt,
            string? AccentColor,
            DateTime? CreatedAt
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
            if (!string.IsNullOrEmpty(Priority) && !prioridades.Contains(Priority.ToLower()))
            {
                throw new ArgumentException("La prioridad no es válida.");
            }

            //Color de acento valido
            var coloresValidos = new[] { "red", "blue", "green", "yellow", "purple", "orange" };
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

            if (CreatedAt.HasValue && CreatedAt > DateTime.UtcNow)
            {
                throw new ArgumentException("La fecha de creación no puede ser posterior a la fecha actual.");
            }
        }

    }
}
