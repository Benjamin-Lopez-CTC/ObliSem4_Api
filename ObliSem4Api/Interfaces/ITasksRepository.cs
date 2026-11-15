using ObliSem4Api.Models;
using ObliSem4Api.DTO.Tasks;

namespace ObliSem4Api.Interfaces
{
    public interface ITasksRepository
    {
        Task<List<Tasks>> GetAll();
    }
}
