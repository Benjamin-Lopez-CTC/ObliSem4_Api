using ObliSem4Api.Interfaces;
using ObliSem4Api.Models;
using ObliSem4Api.DTO.Projects;

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
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                AccentColor = project.AccentColor,
                IsDeleted = project.IsDeleted,
                CreatedAt = project.CreatedAt,

                //Calculo para obtener la cantidad de tareas y la cantidad de tareas completadas
                TasksCount = project.Tasks?.Count ?? 0,
                CompleteTasks = project.Tasks?.Count(t => t.CompletedAt != null) ?? 0

                //Calculo para obtener la cantidad de tareas y la cantidad de tareas completadas, excluyendo las tareas eliminadas
                //TasksCount = project.Tasks?.Count(t => !t.IsDeleted) ?? 0,
                //CompleteTasks = project.Tasks?.Count(t => !t.IsDeleted && t.CompletedAt != null) ?? 0
            };
        }

        public async Task<List<ProjectReadDTO>> GetAll()
        {
            var proyectos = await _repository.GetAll();

            return proyectos.Select(ToReadDto).ToList();
        }
    }
}
