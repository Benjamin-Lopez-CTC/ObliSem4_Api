using ObliSem4Api.DTO.Projects;

namespace ObliSem4Api.Interfaces
{
    public interface IProjectsService
    {
        Task<List<ProjectReadDTO>> GetAll();
    }
}
