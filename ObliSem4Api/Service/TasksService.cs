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

        public async Task<List<TaskReadDto>> GetAll()
        {
            var tareas = await _repository.GetAll();

            return tareas.Select(ToReadDto).ToList();
        }
    }
}
