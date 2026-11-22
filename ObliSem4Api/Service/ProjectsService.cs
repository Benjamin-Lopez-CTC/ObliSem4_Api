using ObliSem4Api.DTO.Filters;
using ObliSem4Api.DTO.Projects;
using ObliSem4Api.Interfaces;
using ObliSem4Api.Models;

namespace ObliSem4Api.Service
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _repository;

        public ProjectsService(IProjectsRepository repository)
        {
            _repository = repository;
        }

        private static ProjectReadDTO ToReadDto(Projects project)
        {
            return new ProjectReadDTO
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                AccentColor = project.AccentColor,
                IsDeleted = project.IsDeleted,
                CreatedAt = project.CreatedAt,

                //Calculo para obtener la cantidad de tareas y la cantidad de tareas completadas
                //TasksCount = project.Tasks?.Count ?? 0,
                //CompleteTasks = project.Tasks?.Count(t => t.CompletedAt != null) ?? 0

                //Calculo para obtener la cantidad de tareas y la cantidad de tareas completadas, excluyendo las tareas eliminadas
                TasksCount = project.Tasks?.Count(t => !t.IsDeleted) ?? 0,
                CompleteTasks = project.Tasks?.Count(t => !t.IsDeleted && t.CompletedAt != null) ?? 0
            };
        }

        private static Projects ToModel(ProjectCreateDTO projectCreateDto, int userId)
        {
            return new Projects
            {
                UserId = userId,
                Name = projectCreateDto.Name,
                Description = projectCreateDto.Description ?? string.Empty,
                Status = projectCreateDto.Status,
                StartDate = projectCreateDto.StartDate,
                EndDate = projectCreateDto.EndDate,
                AccentColor = projectCreateDto.AccentColor,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
            };
        }

        public async Task<List<ProjectReadDTO>> GetAllAsync()
        {
            var proyectos = await _repository.GetAllAsync();

            return proyectos.Select(ToReadDto).ToList();
        }

        public async Task<List<ProjectReadDTO>> GetFilteredAsync(int userId, ProjectFilterDto filter)
        {
            filter.Name = filter.Name?.Trim();
            filter.Description = filter.Description?.Trim();
            filter.Status = filter.Status?.Trim();
            filter.AccentColor = filter.AccentColor?.Trim().ToLower();

            ValidarDatos(
                filter.Name,
                filter.Description,
                filter.Status,
                filter.StartDate,
                filter.EndDate,
                filter.AccentColor,
                filter.CreatedAt
            );

            var proyectos = await _repository.GetFilteredAsync(userId, filter);
            return proyectos.Select(ToReadDto).ToList();
        }

        public async Task<ProjectReadDTO> GetByIdAsync(int id, int userId)
        {
            ValidarId(id);

            var project = await _repository.GetByIdAsync(id);
            if (project == null || project.UserId != userId)
            {
                throw new KeyNotFoundException(
                        $"No se pudo encontrar el proyecto con ID {id} en el usuario actual."
                    );
            }

            return ToReadDto(project);
        }

        public async Task<ProjectReadDTO> CreateAsync(int userId, ProjectCreateDTO dto)
        {
            dto.Name = dto.Name.Trim();
            dto.Description = dto.Description?.Trim();
            dto.AccentColor = dto.AccentColor?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("El nombre es obligatorio.");
            }

            ValidarDatos(
                dto.Name,
                dto.Description,
                dto.Status,
                dto.StartDate,
                dto.EndDate,
                dto.AccentColor,
                null
            );

            var project = ToModel(dto, userId);
            await _repository.CreateAsync(project);
            return ToReadDto(project);
        }

        public async Task UpdateColorAsync(int id, int userId, string? color)
        {
            ValidarId(id);

            color = color?.Trim().ToLower();

            ValidarDatos(accentColor: color);

            var res = await _repository.UpdateColorAsync(id, userId, color);
            if (!res)
            {
                throw new KeyNotFoundException(
                    $"No se encontro el proyecto con el ID {id}");
            }
        }

        public async Task UpdateStatusAsync(int id, int userId, string? status)
        {
            ValidarId(id);

            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException("El estado es obligatorio.");
            }

            status = status?.Trim();

            ValidarDatos(status: status);

            var res = await _repository.UpdateStatusAsync(id, userId, status);
            if (!res)
            {
                throw new KeyNotFoundException(
                    $"No se encontro el proyecto con ID {id}.");
            }
        }

        public async Task SoftDeleteAsync(int id, int userId)
        {
            ValidarId(id);
            var res = await _repository.SoftDeleteAsync(id, userId);
            if (!res)
            {
                throw new KeyNotFoundException(
                    $"No se encontro el proyecto con ID {id}.");
            }
        }

        public async Task RestoreAsync(int id, int userId)
        {
            ValidarId(id);
            var res = await _repository.RestoreAsync(id, userId);
            if (!res)
            {
                throw new KeyNotFoundException(
                    $"No se encontro el proyecto con ID {id}.");
            }
        }

        public async Task HardDeleteAsync(int id)
        {
            ValidarId(id);
            var res = await _repository.HardDeleteAsync(id);
            if (!res)
            {
                throw new KeyNotFoundException(
                    $"No se encontro el proyecto con ID {id}.");
            }
        }

        // Validacion basica de id reutilizable
        private static void ValidarId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(
                    "El ID del proyecto es obligatorio"
                );
            }
        }

        // Validaction de datos reutilizable
        private static void ValidarDatos(
            string? name = null,
            string? description = null,
            string? status = null,
            DateOnly? startDate = null,
            DateOnly? endDate = null,
            string? accentColor = null,
            DateTime? createdAt = null)
        {
            // Longitud de textos
            if (name?.Length > 100)
            {
                throw new ArgumentException("El nombre no puede superar los 100 caracteres.");
            }
            if (description?.Length > 500)
            {
                throw new ArgumentException("La descripción no puede superar los 500 caracteres.");
            }
            // Estado valido
            var estadosValidos = new[] { "pendiente", "en progreso", "completado", "archivado" };
            if (!string.IsNullOrEmpty(status) && !estadosValidos.Contains(status.ToLower()))
            {
                throw new ArgumentException("El estado no es válido.");
            }
            // Color de acento valido
            var coloresValidos = new[] { "red", "blue", "green", "yellow", "purple", "orange" };
            if (!string.IsNullOrEmpty(accentColor) && !coloresValidos.Contains(accentColor.ToLower()))
            {
                throw new ArgumentException("El color de acento no es válido.");
            }
            // Fechas coherentes
            if (startDate.HasValue && endDate.HasValue && endDate < startDate)
            {
                throw new ArgumentException("La fecha de fin no puede ser anterior a la fecha de inicio.");
            }
            if (createdAt.HasValue && createdAt > DateTime.UtcNow)
            {
                throw new ArgumentException("La fecha de creación no puede ser posterior a la fecha actual.");
            }
        }
    }
}
