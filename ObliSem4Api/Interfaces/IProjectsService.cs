using ObliSem4Api.DTO.Filters;
using ObliSem4Api.DTO.Projects;

namespace ObliSem4Api.Interfaces
{
    public interface IProjectsService
    {
        Task<List<ProjectReadDTO>> GetAllAsync();
        Task<List<ProjectReadDTO>> GetFilteredAsync(int userId, ProjectFilterDto filter);
        Task<ProjectReadDTO> GetByIdAsync(int id, int userId);
        Task<ProjectReadDTO> CreateAsync(int userId, ProjectCreateDTO dto);
        Task UpdateColorAsync(int id, int userId, string? color);
        Task UpdateStatusAsync(int id, int userId, string? status);
        Task SoftDeleteAsync(int id, int userId);
        Task RestoreAsync(int id, int userId);
        Task HardDeleteAsync(int id);
    }
}
