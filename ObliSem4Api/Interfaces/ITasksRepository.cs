using ObliSem4Api.DTO.Filters;
using ObliSem4Api.DTO.Tasks;
using ObliSem4Api.Models;

namespace ObliSem4Api.Interfaces
{
    public interface ITasksRepository
    {
        //Get all con filtros opcionales
        Task<PagedResult<TaskReadDto>> GetFilteredAsync(int userId, TaskFilterDto filter);

        //Crea una nueva tarea para el usuario actualmente en sesion.
        Task CreateAsync(Tasks tarea);
    }
}
