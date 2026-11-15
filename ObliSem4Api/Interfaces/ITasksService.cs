using ObliSem4Api.DTO.Tasks;

namespace ObliSem4Api.Interfaces
{
    public interface ITasksService
    {
        Task<List<TaskReadDto>> GetAll();
    }
}
