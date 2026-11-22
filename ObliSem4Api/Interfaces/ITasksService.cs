using ObliSem4Api.DTO.Filters;
using ObliSem4Api.DTO.Tasks;

namespace ObliSem4Api.Interfaces
{
    public interface ITasksService
    {
        //Get all con filtros opcionales
        Task<PagedResult<TaskReadDto>> GetFilteredAsync(int userId, TaskFilterDto filter);

        //Crea una nueva tarea para el usuario actualmente en sesion.
        Task<TaskReadDto> CreateAsync(int userId, TaskCreateDto taskCreateDto);
    }
}
