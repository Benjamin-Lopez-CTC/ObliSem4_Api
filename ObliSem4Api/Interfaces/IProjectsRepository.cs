using ObliSem4Api.Models;
using ObliSem4Api.DTO.Projects;
using ObliSem4Api.DTO.Filters;

namespace ObliSem4Api.Interfaces
{
    public interface IProjectsRepository
    {
        Task<List<Projects>> GetAllAsync();
        Task<List<Projects>> GetFilteredAsync(int userId, ProjectFilterDto dto);
        Task<Projects?> GetByIdAsync(int id);
        Task<bool> UpdateColorAsync(int id, int userId, string? color);
        Task CreateAsync(Projects project);
        Task<bool> UpdateStatusAsync(int id, int userId, string status);
        Task<bool> SoftDeleteAsync(int id, int userId);
        Task<bool> RestoreAsync(int id, int userId);
        Task<bool> HardDeleteAsync(int id);
    }
}
